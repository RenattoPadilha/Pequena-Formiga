using UnityEngine;

public class PhaseNode : MonoBehaviour
{
  [SerializeField] private PhaseData phaseData;
  [SerializeField] private Transform playerAnchor;
  [SerializeField] private Transform tooltipAnchor;

  public PhaseData Data => phaseData;
  public Transform PlayerAnchor => playerAnchor;
  public Transform TooltipAnchor => tooltipAnchor;
  public bool IsValid => phaseData && playerAnchor && tooltipAnchor;

  void OnValidate()
  {
    if (!phaseData) Debug.LogWarning("Phase Data not assigned", this);
    if (!playerAnchor) Debug.LogWarning("Player Anchor not assigned", this);
    if (!tooltipAnchor) Debug.LogWarning("Tooltip Anchor not assigned", this);
  }

  void Awake()
  {
    if (!phaseData)
    {
      Debug.LogError("Phase Data not assigned", this);
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