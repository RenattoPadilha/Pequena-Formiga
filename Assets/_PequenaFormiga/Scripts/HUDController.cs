using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    [SerializeField] private Image healthBarFill;
    [SerializeField] private HealthComponent playerHealth;
    [SerializeField] private TMP_Text healthLabel;
    [SerializeField] private TMP_Text objectiveLabel;

    [Header("Stamina")]
    [SerializeField] private AntStamina playerStamina;
    [SerializeField] private Image staminaBarFill;

    [Header("Stamina Colors")]
    [SerializeField] private Color normalStaminaColor = new Color(0.2f, 0.75f, 0.35f);
    [SerializeField] private Color lowStaminaColor = new Color(0.95f, 0.6f, 0.15f);
    [SerializeField] private Color criticalStaminaColor = new Color(0.85f, 0.2f, 0.2f);

    void Awake()
    {
      bool isValid = true;

      // if (!playerHealth)
      // {
      //   Debug.LogError("Player Health Component not assigned", this);
      //   isValid = false;
      // }
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

    private void Update()
    {
        if (playerStamina == null || staminaBarFill == null)
            return;

        float staminaValue = playerStamina.StaminaNormalized;

        staminaBarFill.fillAmount = staminaValue;

        if (staminaValue <= 0.1f)
        {
            staminaBarFill.color = criticalStaminaColor;
        }
        else if (staminaValue <= 0.3f)
        {
            staminaBarFill.color = lowStaminaColor;
        }
        else
        {
            staminaBarFill.color = normalStaminaColor;
        }
    }

    void Start()
    {
        // Atualiza a vida somente se existir um HealthComponent.
        if (playerHealth != null)
        {
            int currHealth = playerHealth.Current;
            int maxHealth = playerHealth.Max;

            UpdateHealthLabel(currHealth, maxHealth);
        }

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

        if (healthBarFill != null && maxHealth > 0)
        {
            healthBarFill.fillAmount =
                (float)currHealth / maxHealth;
        }
    }
}
