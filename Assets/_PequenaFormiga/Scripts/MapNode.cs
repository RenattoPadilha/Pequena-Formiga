using UnityEngine;

public class LevelNode : MonoBehaviour
{
  [SerializeField] private LevelData levelData;
  [SerializeField] private Transform playerAnchor;
  [SerializeField] private Transform tooltipAnchor;

  public LevelData Data => levelData;
  public Transform PlayerAnchor => playerAnchor;
  public Transform TooltipAnchor => tooltipAnchor;
  public bool IsValid => levelData && playerAnchor && tooltipAnchor;

  void OnValidate()
  {
    if (!levelData) Debug.LogWarning("Level Data not assigned", this);
    if (!playerAnchor) Debug.LogWarning("Player Anchor not assigned", this);
    if (!tooltipAnchor) Debug.LogWarning("Tooltip Anchor not assigned", this);
  }

  void Awake()
  {
    if (!levelData)
    {
      Debug.LogError("Level Data not assigned", this);
    }
    if (!playerAnchor)
    {
      Debug.LogError("Player Anchor Transform not assigned", this);
    }
    if (!tooltipAnchor)
    {
      Debug.LogError("Tooltip Anchor Transform not assigned", this);
    }
  }
}