
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
    }

    private void ReadInput()
    {
        if (Keyboard.current == null)
        {
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

        IsRunning = hasInput && shiftHeld;
    }

    private void FixedUpdate()
    {
        float targetSpeed = IsRunning ? runSpeed : walkSpeed;

        Vector3 targetVelocity = inputDirection * targetSpeed;

        bool hasInput = inputDirection.sqrMagnitude > 0.01f;

        float rate = hasInput ? acceleration : deceleration;

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
}
