
using UnityEngine;

public partial class AntController3d
{
    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundProbeStartOffset = 0.1f;
    [SerializeField] private float groundProbeDistance = 0.15f;

    [SerializeField, Range(0f, 60f)]
    private float maxGroundAngle = 45f;

    public bool IsGrounded { get; private set; }

    private void UpdateGroundedState()
    {
        // Durante a escalada, a formiga não está no chão.
        if (isClimbing || capsule == null)
        {
            IsGrounded = false;
            return;
        }

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

        // Começa ligeiramente acima da posição de contato.
        Vector3 origin =
            lowerSphereCenter + Vector3.up * groundProbeStartOffset;

        float probeRadius = radius * 0.9f;

        float castDistance =
            groundProbeStartOffset
            + groundProbeDistance
            + (radius - probeRadius);

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

        IsGrounded =
            foundGround &&
            Vector3.Dot(hit.normal, Vector3.up) >= minimumGroundDot;

        // Visualização da sondagem na Scene View.
        Debug.DrawRay(
            origin,
            Vector3.down * castDistance,
            IsGrounded ? Color.green : Color.red
        );
    }
}
