
using UnityEngine;

public class AntStamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina = 100f;

    [Header("Consumption")]
    [SerializeField] private float runDrainRate = 15f;
    [SerializeField] private float climbDrainRate = 10f;

    [Header("Recovery")]
    [SerializeField] private float recoveryRate = 20f;
    [SerializeField, Range(0f, 1f)]
    private float runRecoveryThreshold = 0.3f;

    private bool runLocked;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    public float StaminaNormalized =>
        maxStamina > 0f ? currentStamina / maxStamina : 0f;

    public bool IsDepleted => currentStamina <= 0f;
    public bool CanRun => !runLocked;

    private void Awake()
    {
        currentStamina = Mathf.Clamp(
            currentStamina,
            0f,
            maxStamina
        );
    }

    public void ConsumeRun(float deltaTime)
    {
        Consume(runDrainRate * deltaTime);
    }

    public void ConsumeClimb(float deltaTime)
    {
        Consume(climbDrainRate * deltaTime);
    }

    public void Recover(float deltaTime)
    {
        if (currentStamina >= maxStamina)
            return;

        currentStamina = Mathf.Min(
            currentStamina + recoveryRate * deltaTime,
            maxStamina
        );

        if (runLocked &&
            currentStamina >= maxStamina * runRecoveryThreshold)
        {
            runLocked = false;
        }
    }

    private void Consume(float amount)
    {
        if (IsDepleted)
            return;

        currentStamina = Mathf.Max(
            currentStamina - amount,
            0f
        );

        if (IsDepleted)
            runLocked = true;
    }
}
