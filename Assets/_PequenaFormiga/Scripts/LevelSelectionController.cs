using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelSelectionController : MonoBehaviour
{
  [SerializeField] private Transform player;
  [SerializeField] private LevelTooltipController tooltip;
  [SerializeField] private LevelNode[] allLevels;

  private LevelNode selectedLevel;
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
    if (!ValidateLevelArray())
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

    LevelNode startNode = System.Array.Find(allLevels, level => level.Data.LevelNumber == currLevel);

    if (!startNode)
    {
      if (currLevel >= 0)
        Debug.LogWarning($"Level {currLevel} not found on map, using first level", this);
      startNode = allLevels[0];
    }

    SelectLevel (startNode);
  }

  private void SelectLevel(LevelNode node)
  {
    selectedLevel = node;
    player.position = node.PlayerAnchor.position;
    tooltip.transform.position = node.TooltipAnchor.position;
    tooltip.Show(node.Data);
  }

  private void HandleConfirm(InputAction.CallbackContext ctx)
  {
    if (isLoading)
      return;
    if (!selectedLevel)
      return;

    isLoading = true;
    SceneManager.LoadScene(selectedLevel.Data.SceneName);
  } 

  private bool ValidateLevelArray()
  {
    if (!(allLevels.Length > 0))
    {
      Debug.LogError($"Level Array Empty", this);
      return false;
    }

    bool isValid = true;

    HashSet<int> levelNumbers = new HashSet<int>();

    for (int i = 0; i < allLevels.Length; i++)
    {
      var levelNode = allLevels[i];

      if (!levelNode)
      {
        Debug.LogError($"Null in {i+1}° array position", this);
        isValid = false;
        continue;
      }

      if (!levelNode.IsValid || !levelNode.Data.IsValid)
      {
        Debug.LogError($"Level Node isn't configured correctly at {i+1}° array position\nNode Name: {levelNode.name}", this);
        isValid = false;
        continue;
      }
      if (!Application.CanStreamedLevelBeLoaded(levelNode.Data.SceneName))
      {
        Debug.LogError($"Scene Name invalid in {i+1}° array position.\nNode Name: {levelNode.name}\n-Scene Name: {levelNode.Data.SceneName}", this);
        isValid = false;
      }
      if (!levelNumbers.Add(levelNode.Data.LevelNumber))
      {
        Debug.LogError($"Level Number duplicated in {i+1}° array position.\nNode Name: {levelNode.name}\n- Level Number: {levelNode.Data.LevelNumber}", this);
        isValid = false;
      }
    }

    return isValid;
  }
}
