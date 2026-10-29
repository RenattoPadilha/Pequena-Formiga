using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
  [SerializeField] private string levelName;
  [SerializeField] [Min(0)] private int levelNumber = -1;
  [SerializeField] private string sceneName;

  public string LevelName => levelName;
  public int LevelNumber => levelNumber;
  public string SceneName => sceneName;

  public bool IsValid => (!string.IsNullOrEmpty(levelName)) && (levelNumber >= 0) && (!string.IsNullOrEmpty(sceneName));

  void OnValidate()
  {
    if (string.IsNullOrEmpty(levelName)) Debug.LogWarning("Level Name not assigned", this);
    if (levelNumber < 0) Debug.LogWarning("Level Number not assigned", this);
    if (string.IsNullOrEmpty(sceneName)) Debug.LogWarning("Scene Name not assigned", this);
  }
}
