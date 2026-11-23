using UnityEngine;

public partial class AntController3d
{
    private bool isTrackingFall;
    private float fallStartY;

    [Header("Fall Damage")]
    [SerializeField, Min(0f)] private float safeFallDistance = 2.5f;
    [SerializeField, Min(0f)] private float damagePerUnit = 0.5f;
    [SerializeField, Min(0f)] private float minimumFallDistance = 0.25f;
    [SerializeField] private HealthComponent healthComponent;

    private void UpdateFallDetection(bool wasGrounded)
    {
        // A escalada e a transição para o topo não contam como queda.
        if (isClimbing)
            return;

        // Começou a cair ao deixar o chão.
        if (wasGrounded && !IsGrounded && !isTrackingFall)
        {
            BeginFall();
            return;
        }

        // Aterrissou após uma queda.
        if (isTrackingFall && IsGrounded)
        {
            float fallDistance = Mathf.Max(
                0f,
                fallStartY - transform.position.y
            );

            if (fallDistance >= minimumFallDistance)
            {
                float excessDistance = Mathf.Max(
                    0f,
                    fallDistance - safeFallDistance
                );

                int calculatedDamage = Mathf.FloorToInt(
                    excessDistance * damagePerUnit + 0.5f
                );

                Debug.Log(
                    $"Queda encerrada. Distância: {fallDistance:F2} | " +
                    $"Dano calculado: {calculatedDamage}"
                );

                if (calculatedDamage > 0 && healthComponent != null)
                {
                    healthComponent.TakeDamage(calculatedDamage);

                    Debug.Log(
                        $"Vida da formiga: {healthComponent.Current}/{healthComponent.Max}"
                    );
                }
            }

            isTrackingFall = false;
        }
    }

    private void BeginFall()
    {
        isTrackingFall = true;
        fallStartY = transform.position.y;
    }
}