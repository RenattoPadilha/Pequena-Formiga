using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PhaseSelectionController : MonoBehaviour
{
  [SerializeField] private Transform player;
  [SerializeField] private PhaseTooltipController tooltip;
  [SerializeField] private PhaseNode[] allPhases;

  private PhaseNode selectedPhase;
  private bool isLoading = false;

  private PlayerController controls;

  void Awake()
  {
    bool isValid = true;

    if (!player)
    {
      Debug.LogError("Player Transform not assigned", this);
      isValid = false;
    }
    if (!tooltip)
    {
      Debug.LogError("Tooltip Script not assigned", this);
      isValid = false;
    }
    if (!ValidatePhaseArray())
    {
      isValid = false;
    }

    if (!isValid)
    {
      enabled = false;
      return;
    }

    controls = new PlayerController();
  }

  void OnEnable()
  {
    if (controls == null)
      return;
    controls.Map.Interact.performed += HandleConfirm; 
    controls.Map.Enable();
  }

  void OnDisable()
  {
    if (controls == null)
      return;
    controls.Map.Interact.performed -= HandleConfirm; 
    controls.Map.Disable();
  }

  void OnDestroy()
  {
    controls?.Dispose();
  }

  void Start()
  {
    int currLevel = GameSession.LastLevel;

    PhaseNode startNode = System.Array.Find(allPhases, phase => phase.Data.PhaseNumber == currLevel);

    if (!startNode)
    {
      if (currLevel >= 0)
        Debug.LogWarning($"Level {currLevel} not found on map, using first phase", this);
      startNode = allPhases[0];
    }

    SelectPhase (startNode);
  }

  private void SelectPhase(PhaseNode node)
  {
    selectedPhase = node;
    player.position = node.PlayerAnchor.position;
    tooltip.transform.position = node.TooltipAnchor.position;
    tooltip.Show(node.Data);
  }

  private void HandleConfirm(InputAction.CallbackContext ctx)
  {
    if (isLoading)
      return;
    if (!selectedPhase)
      return;

    isLoading = true;
    SceneManager.LoadScene(selectedPhase.Data.SceneName);
  } 

  private bool ValidatePhaseArray()
  {
    if (!(allPhases.Length > 0))
    {
      Debug.LogError($"Phase Array Empty", this);
      return false;
    }

    bool isValid = true;

    HashSet<int> phaseNumbers = new HashSet<int>();

    for (int i = 0; i < allPhases.Length; i++)
    {
      var phaseNode = allPhases[i];

      if (!phaseNode)
      {
        Debug.LogError($"Null in {i+1}° array position", this);
        isValid = false;
        continue;
      }

      if (!phaseNode.IsValid || !phaseNode.Data.IsValid)
      {
        Debug.LogError($"Phase Node isn't configured correctly at {i+1}° array position\nNode Name: {phaseNode.name}", this);
        isValid = false;
        continue;
      }
      if (!Application.CanStreamedLevelBeLoaded(phaseNode.Data.SceneName))
      {
        Debug.LogError($"Scene Name invalid in {i+1}° array position.\nNode Name: {phaseNode.name}\n-Scene Name: {phaseNode.Data.SceneName}", this);
        isValid = false;
      }
      if (!phaseNumbers.Add(phaseNode.Data.PhaseNumber))
      {
        Debug.LogError($"Phase Number duplicated in {i+1}° array position.\nNode Name: {phaseNode.name}\n- Phase Number: {phaseNode.Data.PhaseNumber}", this);
        isValid = false;
      }
    }

    return isValid;
  }
}
