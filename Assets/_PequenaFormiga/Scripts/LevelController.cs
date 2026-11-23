using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
  [SerializeField] private LevelData levelData;
  [SerializeField] private HealthComponent playerHealth;
  [SerializeField] private string mapSceneName;

  void Awake()
  {
    bool isValid = true;

    if (!playerHealth)
    {
      Debug.LogError("Player Health Component not assigned", this);
      isValid = false;
    }
      
    if (!levelData || !levelData.IsValid)
    {
      Debug.LogError("Level Data not assigned or incomplete", this);
      isValid = false;
    }
      
    if (string.IsNullOrEmpty(mapSceneName))
    {
      Debug.LogError("Map Scene Name not assigned", this); 
      isValid = false;
    }

    if (!isValid)
    {
      enabled = false;
      return;
    }
    
    GameSession.SetLastLevel(levelData.LevelNumber);
  }


  void OnEnable()
  {
    if (!playerHealth)
      return;
    
    playerHealth.OnDeath += HandlePlayerDeath;
  }

  void OnDisable()
  {
    if (!playerHealth)
      return;

    playerHealth.OnDeath -= HandlePlayerDeath;
  }

  private void HandlePlayerDeath()
  {
    if (string.IsNullOrEmpty(mapSceneName))
      return;
    //Colocar futuramente uma coroutine que faça uma animação para o jogador (em outro script)
    SceneManager.LoadScene(mapSceneName);
  }
}
