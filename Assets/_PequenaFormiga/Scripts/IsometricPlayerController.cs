using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class IsometricPlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Movement Smoothing")]
    public float acceleration = 12f;
    public float deceleration = 16f;

    private Vector2 currentVelocity;
    private bool isRunning;
    private InputAction sprintAction;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private PlayerController controls;

    [Header("Player Input Settings")]
    public bool isoMovement;
    public Vector2 isoRight = new Vector2(1, 0.5f);
    public Vector2 isoUp = new Vector2(-1, 0.5f);

    [Header("Directional Movement")]
    public Animator playerSpriteAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerController();
        sprintAction = controls.Player.Sprint;
    }

    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => inputVector = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => inputVector = Vector2.zero;
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    
    private void FixedUpdate()
    {
        // 1. Define a direção de movimento.
        Vector2 moveDirection;

        isRunning = sprintAction.IsPressed()
            && inputVector.sqrMagnitude > 0.01f;

        if (!isoMovement)
        {
            moveDirection = inputVector.normalized;
        }
        else
        {
            moveDirection =
                (isoRight * inputVector.x +
                isoUp * inputVector.y).normalized;
        }

        // 2. Define a velocidade máxima desejada.
        float targetSpeed = isRunning ? runSpeed : moveSpeed;

        // Sem direção de entrada, a velocidade desejada é zero.
        Vector2 targetVelocity = moveDirection * targetSpeed;

        // 3. Escolhe a taxa de aceleração ou desaceleração.
        float rate = inputVector.sqrMagnitude > 0.01f
            ? acceleration
            : deceleration;

        // 4. Aproxima suavemente a velocidade atual da desejada.
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            rate * Time.fixedDeltaTime
        );

        // 5. Aplica o movimento.
        rb.MovePosition(
            rb.position + currentVelocity * Time.fixedDeltaTime
        );

        UpdateSpriteDirection(inputVector);
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        direction.Normalize();

        //Get remapped values
        float finalX = (direction.x > 0 ? (direction.x > 0.5f ? 1 : 0) : (direction.x < -0.5f ? -1 : 0));
        float finalY = (direction.y > 0 ? (direction.y > 0.5f ? 1 : 0) : (direction.y < -0.5f ? -1 : 0));

        //Set values to animator
        playerSpriteAnimator.SetFloat("X", finalX);
        playerSpriteAnimator.SetFloat("Y", finalY);
    }
}