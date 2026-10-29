
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class AntController3D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float deceleration = 16f;

    [Header("Climbing")]
    private Vector2 moveInput;
    private Vector3 climbUp;
    private Vector3 climbRight;
    private bool requireInputRelease;
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float climbDetectionDistance = 1.5f;
    [SerializeField] private float surfaceOffset = 0.35f;

    private bool isClimbing;
    private Vector3 climbNormal;
    private Vector3 climbPoint;
    private bool previousUseGravity;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform visual;

    private Rigidbody rb;
    private Vector3 currentHorizontalVelocity;
    private Vector3 inputDirection;

    public bool IsRunning { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints |= RigidbodyConstraints.FreezeRotationX;
        rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        ReadInput();

        if (Keyboard.current != null &&
            Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (isClimbing)
                StopClimbing();
            else
                TryStartClimbing();
        }
    }

    private void ReadInput()
    {
        if (Keyboard.current == null)
        {
            moveInput = Vector2.zero;
            inputDirection = Vector3.zero;
            IsRunning = false;
            return;
        }

        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.wKey.isPressed) z += 1f;

        moveInput = new Vector2(x, z).normalized;

        Vector3 forward = cameraTransform != null
            ? cameraTransform.forward
            : Vector3.forward;

        Vector3 right = cameraTransform != null
            ? cameraTransform.right
            : Vector3.right;

        // Remove the vertical component of the camera directions.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        inputDirection = (right * x + forward * z).normalized;

        bool hasInput = inputDirection.sqrMagnitude > 0.01f;
        bool shiftHeld =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        if (requireInputRelease)
        {
            moveInput = Vector2.zero;
            inputDirection = Vector3.zero;
            IsRunning = false;

            if (x == 0f && z == 0f)
                requireInputRelease = false;

            return;
        }

        IsRunning = hasInput && shiftHeld;
    }

    private void FixedUpdate()
    {
        float targetSpeed = IsRunning ? runSpeed : walkSpeed;

        Vector3 targetVelocity = inputDirection * targetSpeed;

        bool hasInput = inputDirection.sqrMagnitude > 0.01f;

        float rate = hasInput ? acceleration : deceleration;

        if (isClimbing)
        {
            Climb();
            return;
        }

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            rate * Time.fixedDeltaTime
        );

        // Elimina a velocidade residual quando o personagem para.
        if (!hasInput && currentHorizontalVelocity.magnitude < 0.05f)
        {
            currentHorizontalVelocity = Vector3.zero;
        }

        // Atualiza a velocidade horizontal sem alterar a gravidade.
        Vector3 velocity = rb.linearVelocity;
        velocity.x = currentHorizontalVelocity.x;
        velocity.z = currentHorizontalVelocity.z;
        rb.linearVelocity = velocity;

        // Só gira enquanto houver comando de movimento.
        if (hasInput && visual != null)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(inputDirection, Vector3.up);

            visual.rotation = targetRotation;
        }
    }

    
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
        climbPoint = hit.point;

        // Define o eixo vertical sobre a superfície.
        Vector3 referenceUp = cameraTransform != null
            ? cameraTransform.up
            : Vector3.up;

        climbUp = Vector3.ProjectOnPlane(referenceUp, climbNormal);

        // Em superfícies onde a referência é quase perpendicular,
        // utiliza uma direção alternativa.
        if (climbUp.sqrMagnitude < 0.01f)
            climbUp = Vector3.ProjectOnPlane(
                Vector3.forward,
                climbNormal
            );

        climbUp.Normalize();

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


    private void StopClimbing()
    {
        isClimbing = false;

        rb.useGravity = previousUseGravity;

        rb.linearVelocity = Vector3.zero;
        currentHorizontalVelocity = Vector3.zero;

        inputDirection = Vector3.zero;
        moveInput = Vector2.zero;

        IsRunning = false;
        requireInputRelease = true;
    }
    
    
    
    private void Climb()
    {
        // W/S sobem e descem; A/D movimentam lateralmente.
        Vector3 movement =
            climbRight * moveInput.x +
            climbUp * moveInput.y;

        movement = Vector3.ProjectOnPlane(
            movement,
            climbNormal
        );

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        // Mantém o movimento controlado durante a escalada.
        rb.linearVelocity = Vector3.zero;

        rb.MovePosition(
            rb.position +
            movement * climbSpeed * Time.fixedDeltaTime
        );

        // A formiga acompanha a direção do movimento.
        if (movement.sqrMagnitude > 0.001f && visual != null)
        {
            visual.rotation = Quaternion.LookRotation(
                movement.normalized,
                climbNormal
            );
        }
    }


}
