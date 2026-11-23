using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapPlayer : MonoBehaviour
{
  public event Action<MapDirection> OnMoveInput;
  public event Action OnConfirmInput;
  public event Action OnMoveStarted;
  public event Action OnMoveFinished;

  [Min(0.1f)][SerializeField] private float moveSpeed = 12f;
  private PlayerController controls;
  private Vector3 target;
  private bool isMoving;

  private void Awake() 
  {
    controls = new PlayerController();
  }

  void OnEnable()
  {
    if (controls == null)
      return;
    controls.Map.Interact.performed += HandleInteract; 
    controls.Map.Move.performed += HandleMove;
    controls.Map.Enable();
  }

  void OnDisable()
  {
    if (controls == null)
      return;
    controls.Map.Interact.performed -= HandleInteract; 
    controls.Map.Move.performed -= HandleMove;
    controls.Map.Disable();
  }

  void OnDestroy()
  {
    controls?.Dispose();
  }

  void Update()
  {
    if (!isMoving)
      return;
    transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

    if (transform.position == target)
    {
      isMoving = false;
      OnMoveFinished?.Invoke();
    }
  }

  private void HandleInteract(InputAction.CallbackContext ctx)
  {
    if (isMoving)
      return;

    OnConfirmInput?.Invoke();
  }

  private void HandleMove(InputAction.CallbackContext ctx)
  {
    if (isMoving)
      return;

    Vector2 input = ctx.ReadValue<Vector2>();
    if (TryGetDirection(input, out MapDirection direction))
      OnMoveInput?.Invoke(direction);
  }

  private bool TryGetDirection(Vector2 input, out MapDirection direction)
  {
    direction = default;

    if (input.magnitude < 0.6f)
      return false;

    float absX = Mathf.Abs(input.x);
    float absY = Mathf.Abs(input.y);

    if (Mathf.Abs(absX - absY) < 0.1f)
      return false;

    if (absX > absY)
      direction = input.x > 0 ? MapDirection.East : MapDirection.West;
    else
      direction = input.y > 0 ? MapDirection.North : MapDirection.South;

    return true;
  }

  public void MoveTo(Vector3 position)
  {
    if (isMoving) 
      return;
    target = position;
    isMoving = true;
    OnMoveStarted?.Invoke();
    //*Simple Animation runs on Update
  }
  
  public void SetPosition(Vector3 position)
  {
    isMoving = false;
    transform.position = position;
  }
}

