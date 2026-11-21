using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Gera prefabs prontos para uso a partir dos modelos KayKit.
///
/// Para cada pacote:
///  1. Cria (ou atualiza) um material URP/Lit compartilhado usando o atlas de textura do pacote.
///  2. Remapeia os materiais embutidos de cada FBX para esse material (vale também para
///     modelos já colocados nas cenas).
///  3. Cria um Prefab Variant de cada FBX em Prefabs/Environment/&lt;Pacote&gt;/&lt;Categoria&gt;/,
///     marcado como estático e com MeshCollider (exceto categorias sem colisão, como grama).
///
/// Pode ser executado várias vezes: prefabs existentes são sobrescritos mantendo o GUID,
/// então referências nas cenas não quebram.
/// </summary>
public static class KayKitPrefabGenerator
{
    private const string ArtRoot = "Assets/_PequenaFormiga/Art";
    private const string MaterialsRoot = "Assets/_PequenaFormiga/Materials/Environment";
    private const string PrefabsRoot = "Assets/_PequenaFormiga/Prefabs/Environment";

    private struct Pack
    {
        public string Name;            // nome curto usado nas pastas de saída
        public string ModelFolder;     // pasta com os .fbx
        public string TexturePath;     // atlas de cores do pacote
        public HashSet<string> NoColliderCategories;
    }

    private static readonly Pack[] Packs =
    {
        new Pack
        {
            Name = "Forest",
            ModelFolder = ArtRoot + "/KayKit_Forest_Nature_Pack_1.0_FREE/Assets/fbx",
            TexturePath = ArtRoot + "/KayKit_Forest_Nature_Pack_1.0_FREE/Assets/fbx/forest_texture.png",
            NoColliderCategories = new HashSet<string> { "Grass" },
        },
        new Pack
        {
            Name = "Dungeon",
            ModelFolder = ArtRoot + "/KayKit_Dungeon_Pack_1.1_FREE/Assets/fbx",
            TexturePath = ArtRoot + "/KayKit_Dungeon_Pack_1.1_FREE/Assets/fbx/dungeon_texture.png",
            NoColliderCategories = new HashSet<string> { "banner", "coin", "candle", "bottle", "plate", "keyring", "key" },
        },
    };

    [MenuItem("Pequena Formiga/Assets/Gerar Prefabs KayKit")]
    public static void GenerateAll()
    {
        int total = 0;
        try
        {
            foreach (var pack in Packs)
                RemapMaterials(pack);

            foreach (var pack in Packs)
                total += GeneratePrefabs(pack);

            AssetDatabase.SaveAssets();
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
        Debug.Log($"[KayKitPrefabGenerator] {total} prefabs gerados em {PrefabsRoot}");
    }

    // ---------------------------------------------------------------- Materiais

    private static Material GetOrCreateMaterial(Pack pack)
    {
        EnsureFolder(MaterialsRoot);
        string path = $"{MaterialsRoot}/KayKit_{pack.Name}.mat";

        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                Debug.LogError("[KayKitPrefabGenerator] Shader URP/Lit não encontrado.");
                return null;
            }
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, path);
        }

        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(pack.TexturePath);
        if (tex == null)
            Debug.LogWarning($"[KayKitPrefabGenerator] Textura não encontrada: {pack.TexturePath}");

        mat.SetTexture("_BaseMap", tex);
        mat.SetColor("_BaseColor", Color.white);
        mat.SetFloat("_Smoothness", 0.1f);
        mat.SetFloat("_Metallic", 0f);
        mat.enableInstancing = true; // muitos objetos iguais (árvores, pedras) → GPU instancing
        EditorUtility.SetDirty(mat);
        return mat;
    }

    private static void RemapMaterials(Pack pack)
    {
        var mat = GetOrCreateMaterial(pack);
        if (mat == null) return;
        AssetDatabase.SaveAssets();

        foreach (var modelPath in FindModels(pack))
        {
            var importer = AssetImporter.GetAtPath(modelPath) as ModelImporter;
            if (importer == null) continue;

            bool changed = false;
            foreach (var embedded in AssetDatabase.LoadAllAssetsAtPath(modelPath))
            {
                if (embedded is not Material embeddedMat) continue;
                var id = new AssetImporter.SourceAssetIdentifier(typeof(Material), embeddedMat.name);
                importer.AddRemap(id, mat);
                changed = true;
            }

            // Modelos estáticos de cenário não precisam de animação.
            if (importer.importAnimation)
            {
                importer.importAnimation = false;
                changed = true;
            }

            if (changed)
                importer.SaveAndReimport();
        }
    }

    // ---------------------------------------------------------------- Prefabs

    private static int GeneratePrefabs(Pack pack)
    {
        var models = FindModels(pack);
        for (int i = 0; i < models.Count; i++)
        {
            string modelPath = models[i];
            string name = Path.GetFileNameWithoutExtension(modelPath);
            string category = GetCategory(name);

            EditorUtility.DisplayProgressBar($"KayKit {pack.Name}", name, (float)i / models.Count);

            var model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
            if (model == null) continue;

            string folder = $"{PrefabsRoot}/{pack.Name}/{category}";
            EnsureFolder(folder);

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
            try
            {
                instance.name = name;
                instance.transform.position = Vector3.zero;
                GameObjectUtility.SetStaticEditorFlags(instance, StaticFlagsForEnvironment);

                foreach (var child in instance.GetComponentsInChildren<Transform>(true))
                    GameObjectUtility.SetStaticEditorFlags(child.gameObject, StaticFlagsForEnvironment);

                if (!pack.NoColliderCategories.Contains(category))
                {
                    foreach (var mf in instance.GetComponentsInChildren<MeshFilter>(true))
                    {
                        if (mf.GetComponent<Collider>() != null) continue;
                        var col = mf.gameObject.AddComponent<MeshCollider>();
                        col.sharedMesh = mf.sharedMesh;
                    }
                }

                PrefabUtility.SaveAsPrefabAsset(instance, $"{folder}/{name}.prefab");
            }
            finally
            {
                Object.DestroyImmediate(instance);
            }
        }
        return models.Count;
    }

    private const StaticEditorFlags StaticFlagsForEnvironment =
        StaticEditorFlags.ContributeGI |
        StaticEditorFlags.OccluderStatic |
        StaticEditorFlags.OccludeeStatic |
        StaticEditorFlags.BatchingStatic |
        StaticEditorFlags.ReflectionProbeStatic;

    // ---------------------------------------------------------------- Utilidades

    private static List<string> FindModels(Pack pack)
    {
        var result = new List<string>();
        foreach (var guid in AssetDatabase.FindAssets("t:Model", new[] { pack.ModelFolder }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
                result.Add(path);
        }
        result.Sort();
        return result;
    }

    // "Tree_1_A_Color1" → "Tree", "floor_tile_large" → "floor"
    private static string GetCategory(string modelName)
    {
        int idx = modelName.IndexOf('_');
        return idx > 0 ? modelName.Substring(0, idx) : modelName;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        string parent = Path.GetDirectoryName(path).Replace('\\', '/');
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
    }
}
