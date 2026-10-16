using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
  // -1 means not configured. Once changed, the editor doesn't allow it to be set to -1 again, since it has already been configured
  [Min(0)][SerializeField] private int lvlNumber = -1;
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
      
    if (lvlNumber < 0)
    {
      Debug.LogError("Level Number not assigned", this);
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
    
    GameSession.SetLastLevel(lvlNumber);
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
