using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelTooltipController : MonoBehaviour
{
  [SerializeField] private CanvasGroup tooltip; 
  [SerializeField] private TMP_Text levelNameLabel;
  [SerializeField] private TMP_Text actionLabel;

  private bool isValid = true;

  void OnValidate()
  {
    if (!tooltip) Debug.LogWarning("Tooltip CanvasGroup not assigned", this);
    if (!levelNameLabel) Debug.LogWarning("Level Name TMP_Text not assigned", this);
    if (!actionLabel) Debug.LogWarning("Action TMP_Text not assigned", this);
  }

  void Awake()
  {
    if (!tooltip)
    {
      Debug.LogError("Tooltip CanvasGroup not assigned", this);
      isValid = false;
    }
    if (!levelNameLabel)
    {
      Debug.LogError("Level Name TMP_Text not assigned", this);
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

  public void Show(LevelData levelData)
  {
    if (!isValid)
      return;
    if (!levelData)
    {
      Debug.LogError("Tooltip Show called with null LevelData", this);
      return;
    }
    levelNameLabel.text = $"Fase {levelData.LevelNumber}: {levelData.LevelName}";
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
