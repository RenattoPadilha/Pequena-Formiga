using UnityEngine;

public partial class AntController3d
{
    [Header("Climbing")]
    private Vector3 climbUp;
    private Vector3 climbRight;
    
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float climbDetectionDistance = 1.5f;
    private bool isClimbing;
    private Vector3 climbNormal;
    private bool previousUseGravity;

    private Collider climbSurfaceCollider;

    [Header("Climbing Exhaustion")]
    [SerializeField] private float slipDuration = 0.8f;
    [SerializeField] private float slipSpeed = 1.5f;
    [SerializeField] private float slipDetachSpeed = 1.5f;
    [SerializeField, Range(0.1f, 1f)]
    private float slipControlMultiplier = 0.35f;

    private bool isSlipping;
    private float slipTimer;

    private void TryStartClimbing()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Vector3 direction = visual != null
            ? visual.forward
            : transform.forward;

        if (!Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            climbDetectionDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            return;
        }

        if (hit.collider.transform.IsChildOf(transform))
            return;

        isClimbing = true;

        climbNormal = hit.normal;
        climbSurfaceCollider = hit.collider;

        // Define a vertical da escalada usando a vertical do mundo.
        climbUp = Vector3.ProjectOnPlane(
            Vector3.up,
            climbNormal
        ).normalized;

        // Define o eixo lateral sobre a superfície.
        climbRight = Vector3.Cross(
            climbNormal,
            climbUp
        ).normalized;

        previousUseGravity = rb.useGravity;
        rb.useGravity = false;

        rb.linearVelocity = Vector3.zero;
        currentHorizontalVelocity = Vector3.zero;

        // A frente do modelo aponta para cima da superfície,
        // enquanto o eixo superior local fica alinhado à normal.
        if (visual != null)
        {
            visual.rotation = Quaternion.LookRotation(
                climbUp,
                climbNormal
            );
        }
    }

    private void Climb()
    {
        // Durante o escorregamento, ignora os comandos do jogador.
        if (isSlipping)
        {
            UpdateSlip();
            return;
        }

        // Inicia o escorregamento quando o fôlego acaba.
        if (stamina.IsDepleted)
        {
            StartSlip();
            UpdateSlip();
            return;
        }

        bool hasContact = UpdateSurfaceContact();

        Vector3 movement =
            climbRight * moveInput.x +
            climbUp * moveInput.y;

        movement = Vector3.ProjectOnPlane(movement, climbNormal);

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        Vector3 targetPosition =
            rb.position +
            movement * climbSpeed * Time.fixedDeltaTime;
        
        // Proteção das bordas verticais da parede.
        float verticalMovement = Vector3.Dot(movement, climbUp);

        if (verticalMovement > 0.001f)
        {
            // Verifica se ainda existe parede acima da posição desejada.
            Vector3 verticalTarget =
                rb.position +
                climbUp *
                verticalMovement *
                climbSpeed *
                Time.fixedDeltaTime;

            if (!HasWallAtVerticalPosition(verticalTarget, true))
            {
                movement = Vector3.ProjectOnPlane(movement, climbUp);
            }
        }
        else if (verticalMovement < -0.001f)
        {
            // Verifica se ainda existe parede abaixo da posição desejada.
            Vector3 verticalTarget =
                rb.position +
                climbUp *
                verticalMovement *
                climbSpeed *
                Time.fixedDeltaTime;

            if (!HasWallAtVerticalPosition(verticalTarget, false))
            {
                movement = Vector3.ProjectOnPlane(movement, climbUp);
            }
        }

        // Atualiza a posição-alvo após as verificações.
        targetPosition =
            rb.position +
            movement *
            climbSpeed *
            Time.fixedDeltaTime;

        // Proteção lateral, inclusive quando o contato direto foi perdido.
        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            if (!HasWallAtPosition(
                targetPosition,
                moveInput.y > 0.01f
            ))
            {
                // Remove o movimento lateral, preservando o vertical.
                movement = Vector3.ProjectOnPlane(
                    movement,
                    climbRight
                );

                movement = Vector3.ProjectOnPlane(
                    movement,
                    climbNormal
                );
            }
        }

        // Consome fôlego somente quando existe movimento na parede.
        if (movement.sqrMagnitude > 0.001f)
        {
            stamina.ConsumeClimb(Time.fixedDeltaTime);
        }

        rb.linearVelocity = Vector3.zero;

        rb.MovePosition(
            rb.position +
            movement * climbSpeed * Time.fixedDeltaTime
        );

        if (movement.sqrMagnitude > 0.001f && visual != null)
        {
            visual.rotation = Quaternion.LookRotation(
                movement.normalized,
                climbNormal
            );
        }
    }

    private bool UpdateSurfaceContact()
    {
        Vector3 colliderCenter =
            transform.TransformPoint(capsule.center);

        Vector3 origin =
            colliderCenter + climbNormal * 0.05f;

        Vector3 direction = -climbNormal;

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            direction,
            surfaceCheckDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(transform) ||
                hit.collider.transform == transform)
            {
                continue;
            }

            if (hit.collider != climbSurfaceCollider)
                continue;

            climbNormal = hit.normal;

            return true;
        }

        return false;
    }

    private void StartSlip()
    {
        isSlipping = true;
        slipTimer = 0f;

        rb.linearVelocity = Vector3.zero;

        Debug.Log("Formiga exausta: iniciando escorregamento.");
    }

    private void UpdateSlip()
    {
        slipTimer += Time.fixedDeltaTime;

        // Movimento controlado pelo jogador.
        Vector3 playerMovement =
            climbRight * moveInput.x +
            climbUp * moveInput.y;

        playerMovement = Vector3.ProjectOnPlane(
            playerMovement,
            climbNormal
        );

        if (playerMovement.sqrMagnitude > 1f)
            playerMovement.Normalize();

        // Movimento automático para baixo.
        Vector3 slipMovement = -climbUp * slipSpeed;

        Vector3 totalMovement =
            slipMovement +
            playerMovement * climbSpeed * slipControlMultiplier;

        // Separa o movimento vertical e lateral.
        float verticalSpeed = Vector3.Dot(
            totalMovement,
            climbUp
        );

        float lateralSpeed = Vector3.Dot(
            totalMovement,
            climbRight
        );

        Vector3 targetPosition = rb.position;

        // Verifica primeiro o deslocamento lateral.
        if (Mathf.Abs(lateralSpeed) > 0.001f)
        {
            Vector3 lateralTarget =
                targetPosition +
                climbRight * lateralSpeed * Time.fixedDeltaTime;

            if (HasWallAtPosition(lateralTarget))
            {
                targetPosition = lateralTarget;
            }
            else
            {
                lateralSpeed = 0f;
            }
        }

        // Verifica o deslocamento vertical.
        if (Mathf.Abs(verticalSpeed) > 0.001f)
        {
            Vector3 verticalTarget =
                targetPosition +
                climbUp * verticalSpeed * Time.fixedDeltaTime;

            bool movingUp = verticalSpeed > 0f;

            if (HasWallAtVerticalPosition(verticalTarget, movingUp))
            {
                targetPosition = verticalTarget;
            }
            else
            {
                verticalSpeed = 0f;
            }
        }

        

        // Aplica somente os movimentos autorizados pelas sondas.
        rb.MovePosition(targetPosition);

        Vector3 allowedMovement =
            climbUp * verticalSpeed +
            climbRight * lateralSpeed;

        if (visual != null && allowedMovement.sqrMagnitude > 0.001f)
        {
            visual.rotation = Quaternion.LookRotation(
                allowedMovement.normalized,
                climbNormal
            );
        }

        // O temporizador continua funcionando mesmo nas bordas.
        if (slipTimer >= slipDuration)
        {
            FinishSlip();
        }
    }

    private bool HasWallAtVerticalPosition(
        Vector3 position,
        bool checkUpperEdge)
    {
        if (capsule == null || climbSurfaceCollider == null)
            return false;

        float verticalScale = Mathf.Abs(transform.lossyScale.y);

        float horizontalScale = Mathf.Max(
            Mathf.Abs(transform.lossyScale.x),
            Mathf.Abs(transform.lossyScale.z)
        );

        float halfHeight =
            capsule.height * verticalScale * 0.5f;

        float radius =
            capsule.radius * horizontalScale;

        // Distância entre o centro e a região das extremidades da cápsula.
        float segment = Mathf.Max(0f, halfHeight - radius);

        Vector3 colliderCenterOffset =
            transform.TransformPoint(capsule.center) - transform.position;

        Vector3 colliderCenter =
            position + colliderCenterOffset;

        // Usa um ponto próximo à extremidade da cápsula.
        // O segmento pode ser zero quando Height == 2 * Radius.
        float probeOffset = radius * 0.9f;

        Vector3 probePosition = checkUpperEdge
            ? colliderCenter + climbUp * probeOffset
            : colliderCenter - climbUp * probeOffset;

        return CheckWallRay(probePosition);
    }

    private void FinishSlip()
    {
        isSlipping = false;
        isClimbing = false;

        rb.useGravity = previousUseGravity;

        // Desce e se afasta da parede.
        rb.linearVelocity =
            -climbUp * slipSpeed +
            climbNormal * slipDetachSpeed;

        currentHorizontalVelocity = Vector3.zero;

        inputDirection = Vector3.zero;
        moveInput = Vector2.zero;

        BeginFall();

        IsRunning = false;
        requireInputRelease = true;

        climbSurfaceCollider = null;

        Debug.Log("Formiga desprendida da parede por exaustão.");
    }
}
