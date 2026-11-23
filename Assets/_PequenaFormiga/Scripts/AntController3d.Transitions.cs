using UnityEngine;

public partial class AntController3d
{
    private bool TryTransitionToTop()
    {
        
        if (!TryDetectTopSurface(out RaycastHit topHit))
        {
            Debug.Log("Transição cancelada: nenhum chão encontrado.");
            return false;
        }

        // Calcula quanto a formiga precisa subir
        // para posicionar a base do collider sobre o chão.
        float clearance = 0.02f;

        float halfHeight =
            capsule.height * 0.5f * transform.lossyScale.y;

        float radius =
            capsule.radius * transform.lossyScale.x;

        float supportDistance = halfHeight + radius;

        Vector3 targetPosition =
            topHit.point
            + Vector3.up * (supportDistance + clearance)
            - (transform.TransformPoint(capsule.center) - transform.position);

        // Posiciona a formiga sobre a plataforma.
        rb.position = targetPosition;

        // Encerra a escalada e restaura a gravidade.
        isClimbing = false;
        isSlipping = false;
        slipTimer = 0f;
        rb.useGravity = previousUseGravity;

        rb.linearVelocity = Vector3.zero;
        currentHorizontalVelocity = Vector3.zero;

        inputDirection = Vector3.zero;
        moveInput = Vector2.zero;

        IsRunning = false;
        requireInputRelease = true;

        // Restaura a orientação terrestre.
        if (visual != null)
        {
            Vector3 forward = Vector3.ProjectOnPlane(
                -climbNormal,
                Vector3.up
            ).normalized;

            if (forward.sqrMagnitude > 0.001f)
            {
                visual.rotation = Quaternion.LookRotation(
                    forward,
                    Vector3.up
                );
            }
        }

        Debug.Log(
            $"Transição concluída! Plataforma: {topHit.collider.name}"
        );

        return true;
    }



    private void TryTransitionToGround()
    {
        if (capsule == null || rb == null)
            return;

        float verticalScale = Mathf.Abs(transform.lossyScale.y);

        float horizontalScale = Mathf.Max(
            Mathf.Abs(transform.lossyScale.x),
            Mathf.Abs(transform.lossyScale.z)
        );

        float radius = capsule.radius * horizontalScale;

        float halfHeight = Mathf.Max(
            capsule.height * verticalScale * 0.5f,
            radius
        );

        Vector3 colliderCenter =
            transform.TransformPoint(capsule.center);

        // Centro da extremidade inferior da cápsula.
        Vector3 lowerSphereCenter =
            colliderCenter - Vector3.up * (halfHeight - radius);

        Vector3 origin =
            lowerSphereCenter + Vector3.up * groundProbeStartOffset;

        float probeRadius = radius * 0.9f;

        float castDistance =
            groundProbeStartOffset +
            groundProbeDistance +
            (radius - probeRadius);

        bool foundGround = Physics.SphereCast(
            origin,
            probeRadius,
            Vector3.down,
            out RaycastHit hit,
            castDistance,
            groundLayer,
            QueryTriggerInteraction.Collide
        );

        float minimumGroundDot =
            Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);

        if (!foundGround ||
            Vector3.Dot(hit.normal, Vector3.up) < minimumGroundDot)
        {
            Debug.Log("Saída pela base cancelada: chão não encontrado.");
            return;
        }

        // Calcula a posição para apoiar a cápsula sobre o chão.
        float clearance = 0.02f;

        Vector3 colliderCenterOffset =
            transform.TransformPoint(capsule.center) - transform.position;

        Vector3 targetPosition =
            hit.point +
            Vector3.up * (halfHeight + clearance) -
            colliderCenterOffset;

        // Posiciona a formiga sobre o chão.
        rb.position = targetPosition;

        // Encerra a escalada.
        isClimbing = false;
        isSlipping = false;
        slipTimer = 0f;

        rb.useGravity = previousUseGravity;

        rb.linearVelocity = Vector3.zero;
        currentHorizontalVelocity = Vector3.zero;

        inputDirection = Vector3.zero;
        moveInput = Vector2.zero;

        IsRunning = false;
        requireInputRelease = true;

        // Restaura a orientação terrestre.
        if (visual != null)
        {
            Vector3 forward = Vector3.ProjectOnPlane(
                climbNormal,
                Vector3.up
            ).normalized;

            if (forward.sqrMagnitude > 0.001f)
            {
                visual.rotation = Quaternion.LookRotation(
                    forward,
                    Vector3.up
                );
            }
        }

        Debug.Log(
            $"Saída pela base concluída! Chão: {hit.collider.name}"
        );
    }

}
