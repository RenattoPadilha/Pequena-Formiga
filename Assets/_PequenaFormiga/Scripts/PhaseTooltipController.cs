using TMPro;
using UnityEngine;

public class PhaseTooltipController : MonoBehaviour
{
  [SerializeField] private CanvasGroup tooltip; 
  [SerializeField] private TMP_Text phaseNameLabel;
  [SerializeField] private TMP_Text actionLabel;

  private bool isValid = true;

  void OnValidate()
  {
    if (!tooltip) Debug.LogWarning("Tooltip CanvasGroup not assigned", this);
    if (!phaseNameLabel) Debug.LogWarning("Phase Name TMP_Text not assigned", this);
    if (!actionLabel) Debug.LogWarning("Action TMP_Text not assigned", this);
  }

  void Awake()
  {
    if (!tooltip)
    {
      Debug.LogError("Tooltip CanvasGroup not assigned", this);
      isValid = false;
    }
    if (!phaseNameLabel)
    {
      Debug.LogError("Phase Name TMP_Text not assigned", this);
      isValid = false;
    }
    if (!actionLabel)
    {
      Debug.LogError("Action TMP_Text not assigned", this);
      isValid = false;
    }    

    if (isValid)
      Hide();
  }

  public void Show (PhaseData phaseData)
  {
    if (!phaseData)
    {
      Debug.LogWarning("Tooltip Show called with null PhaseData", this);
      return;
    }
    if (!isValid)
      return;
    phaseNameLabel.text = $"Fase {phaseData.PhaseNumber}: {phaseData.PhaseName}";
    actionLabel.text = "Pressione para Entrar";
    tooltip.alpha = 1;
  }

  public void Hide()
  {
    if (!isValid)
      return;
    tooltip.alpha = 0;
  }
}
