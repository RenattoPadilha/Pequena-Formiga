using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class IsometricPlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;

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
        if (!isoMovement)
        {
            Vector2 moveDir = inputVector.normalized;
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 isoDirection = (isoRight * inputVector.x + isoUp * inputVector.y).normalized;
            rb.MovePosition(rb.position + isoDirection * moveSpeed * Time.fixedDeltaTime);
        }

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