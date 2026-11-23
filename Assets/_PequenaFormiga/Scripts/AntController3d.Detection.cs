using UnityEngine;

public partial class AntController3d
{
    [Header("Top Detection")]
    [SerializeField] private float topProbeForwardOffset = 0.15f;
    [SerializeField] private float topProbeUpOffset = 0.2f;
    [SerializeField] private float topProbeDistance = 0.8f;

    [Header("Lateral Edge Detection")]
    [SerializeField] private float lateralProbeStartOffset = 0.6f;
    [SerializeField] private float lateralProbeDistance = 1.2f;

    [Header("Surface Following")]
    [SerializeField] private float surfaceCheckDistance = 0.8f;

    private bool HasWallAtPosition(
        Vector3 position,
        bool checkUpperEdge = false)
    {
        if (capsule == null || climbSurfaceCollider == null)
            return false;

        float horizontalScale = Mathf.Max(
            Mathf.Abs(transform.lossyScale.x),
            Mathf.Abs(transform.lossyScale.z)
        );

        float halfWidth = capsule.radius * horizontalScale;

        float probeOffset = halfWidth * 0.85f;

        Vector3 colliderCenterOffset =
            transform.TransformPoint(capsule.center) - transform.position;

        Vector3 colliderCenter = position + colliderCenterOffset;

        // Posiciona as sondas um pouco abaixo do centro da cápsula.
        float verticalScale = Mathf.Abs(transform.lossyScale.y);

        float probeHeightOffset =
            capsule.height * verticalScale * 0.35f;

        colliderCenter += climbUp *
            (checkUpperEdge ? probeHeightOffset : -probeHeightOffset);

        Vector3 leftPosition =
            colliderCenter - climbRight * probeOffset;

        Vector3 rightPosition =
            colliderCenter + climbRight * probeOffset;
        
        Debug.Log(
            $"SONDAS | " +
            $"Centro: {colliderCenter} | " +
            $"Esquerda: {leftPosition} | " +
            $"Direita: {rightPosition} | " +
            $"Normal: {climbNormal} | " +
            $"Direção: {-climbNormal}"
        );
        
        Debug.DrawRay(
            leftPosition,
            -climbNormal * 2f,
            Color.cyan
        );

        Debug.DrawRay(
            rightPosition,
            -climbNormal * 2f,
            Color.magenta
        );

        Debug.DrawRay(
            leftPosition + climbNormal * lateralProbeStartOffset,
            -climbNormal * lateralProbeDistance,
            Color.cyan
        );

        Debug.DrawRay(
            rightPosition + climbNormal * lateralProbeStartOffset,
            -climbNormal * lateralProbeDistance,
            Color.magenta
        );

        bool leftHasWall = CheckWallRay(leftPosition);
        bool rightHasWall = CheckWallRay(rightPosition);

        Debug.Log(
            $"Lateral | Esquerda: {leftHasWall} | " +
            $"Direita: {rightHasWall} | " +
            $"Posição: {position}"
        );

        return leftHasWall && rightHasWall;
    }

    private bool CheckWallRay(Vector3 position)
    {
        Vector3 origin =
            position + climbNormal * lateralProbeStartOffset;

        Vector3 direction = -climbNormal;

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            direction,
            lateralProbeDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (RaycastHit hit in hits)
        {
            // Ignora qualquer collider pertencente à formiga.
            if (hit.collider.transform.IsChildOf(transform) ||
                hit.collider.transform == transform)
            {
                continue;
            }

            // Só considera a parede que iniciou a escalada.
            if (hit.collider == climbSurfaceCollider)
            {
                Debug.DrawRay(
                    origin,
                    direction * hit.distance,
                    Color.green
                );

                return true;
            }
        }

        Debug.DrawRay(
            origin,
            direction * lateralProbeDistance,
            Color.red
        );

        return false;
    }

    private bool TryDetectTopSurface(out RaycastHit topHit)
    {
        topHit = default;

        if (capsule == null)
            return false;

        float verticalScale = Mathf.Abs(transform.lossyScale.y);

        float horizontalScale = Mathf.Max(
            Mathf.Abs(transform.lossyScale.x),
            Mathf.Abs(transform.lossyScale.z)
        );

        float halfHeight = capsule.height * verticalScale * 0.5f;
        float radius = capsule.radius * horizontalScale;

        Vector3 colliderCenter =
            transform.TransformPoint(capsule.center);

        Vector3 origin =
            colliderCenter
            + climbUp * (halfHeight + topProbeUpOffset)
            - climbNormal * (radius + topProbeForwardOffset);

        Debug.DrawRay(
            origin,
            Vector3.down * 3f,
            Color.brown,
            5f
        );

        Debug.DrawRay(
            origin,
            climbNormal * 2f,
            Color.gray,
            5f
        );

        Debug.Log($"TOP PROBE EXECUTADO | Origem: {origin}");

        RaycastHit[] upwardHits = Physics.RaycastAll(
            origin,
            Vector3.up,
            3f,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        Debug.Log($"COLLIDERS ACIMA: {upwardHits.Length}");

        Debug.DrawRay(
            origin,
            Vector3.up * 3f,
            Color.magenta,
            1f
        );

        foreach (RaycastHit hit in upwardHits)
        {
            if (hit.collider.transform.IsChildOf(transform) ||
                hit.collider.transform == transform)
                continue;

            Debug.Log(
                $"Objeto acima da formiga: {hit.collider.name} | " +
                $"Distância: {hit.distance:F3} | " +
                $"Altura: {hit.point.y:F3}"
            );

            Debug.DrawLine(origin, hit.point, Color.magenta, 1f);
        }

        Debug.DrawRay(
            origin,
            Vector3.down * topProbeDistance,
            Color.yellow,
            1f
        );

        Debug.DrawRay(
            origin,
            climbNormal * 0.5f,
            Color.blue,
            1f
        );

        Debug.DrawRay(
            origin,
            climbUp * 0.5f,
            Color.cyan,
            1f
        );

        Debug.Log(
            $"Origem Top Probe: {origin} | " +
            $"ClimbUp: {climbUp} | " +
            $"ClimbNormal: {climbNormal}"
        );

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            Vector3.down,
            topProbeDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        float closestDistance = float.MaxValue;
        bool foundSurface = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform) ||
                hit.collider.transform == transform)
            {
                continue;
            }

            // Aceita apenas superfícies predominantemente horizontais.
            if (Vector3.Dot(hit.normal, Vector3.up) < 0.7f)
                continue;

            // A superfície precisa estar acima do centro do collider
            // na direção de escalada.
            float heightAboveAnt = Vector3.Dot(
                hit.point - colliderCenter,
                climbUp
            );

            if (heightAboveAnt <= 0.05f)
                continue;

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                topHit = hit;
                foundSurface = true;
            }
        }

        if (foundSurface)
        {
            Debug.DrawLine(origin, topHit.point, Color.green, 5.0f);

            Debug.Log(
                $"Superfície detectada: {topHit.collider.name} | " +
                $"Ponto: {topHit.point} | " +
                $"Distância: {topHit.distance:F3}"
            );
                }
        else
        {
            Debug.DrawRay(
                origin,
                Vector3.down * topProbeDistance,
                Color.red,
                0.1f
            );
        }

        return foundSurface;
    }
}

