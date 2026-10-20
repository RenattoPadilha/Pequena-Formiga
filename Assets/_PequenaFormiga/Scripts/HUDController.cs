using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
  [SerializeField] private HealthComponent playerHealth;
  [SerializeField] private TMP_Text healthLabel;
  [SerializeField] private TMP_Text objectiveLabel;

  void Awake()
  {
    bool isValid = true;

    if (!playerHealth)
    {
      Debug.LogError("Player Health Component not assigned", this);
      isValid = false;
    }
    if (!healthLabel)
    {
      Debug.LogError("Health Label not assigned", this);
      isValid = false;
    }
    if (!objectiveLabel)
    {
      Debug.LogError("Objective Label not assigned", this);
      isValid = false;
    }
    //TODO: Configurar Objective Trigger quando disponível

    if (!isValid)
    {
      enabled = false;
      return;
    }
  }

  void Start()
  {
    int currHealth = playerHealth.Current;
    int maxHealth = playerHealth.Max;

    UpdateHealthLabel(currHealth,maxHealth);

    //TODO: Configurar Objective Label quando disponível
    objectiveLabel.text = "Objetivo: Pegar Comida";
  }

  void OnEnable()
  {
    if (!playerHealth)
      return;
    playerHealth.OnHealthChanged += HandleHealthChanged;
    //TODO: Configurar Objective Trigger quando disponível
  }

  void OnDisable()
  {
    if (!playerHealth)
      return;
    playerHealth.OnHealthChanged -= HandleHealthChanged;
    //TODO: Configurar Objective Trigger quando disponível
  }

  private void HandleHealthChanged(int currHealth, int maxHealth)
  {
    UpdateHealthLabel(currHealth,maxHealth);
  }

  private void UpdateHealthLabel(int currHealth, int maxHealth)
  {
    healthLabel.text = $"Vida: {currHealth}/{maxHealth}";
  } 
  }
