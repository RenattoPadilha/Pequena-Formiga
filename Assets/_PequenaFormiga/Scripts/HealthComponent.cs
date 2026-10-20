using UnityEngine;
using System;

public class HealthComponent : MonoBehaviour
{
  private int currHp;
  [Min(1)][SerializeField] private int maxHp = 2;

  public bool IsDead => currHp <= 0;
  public int Current => currHp;
  public int Max => maxHp;


  public event Action<int> OnDamageTaken;
  public event Action OnDeath;
  public event Action<int,int> OnHealthChanged;

  [ContextMenu("Teste 1: Dano 1")] 
  private void TestDamage() => TakeDamage(1);

  [ContextMenu("Teste 2: Cura 1")] 
  private void TestHeal() => Heal(1);

  void Awake()
  {
    currHp = maxHp;
  }

  public void TakeDamage(int damage)
  {
    if (damage <= 0)
    {
        Debug.LogWarning($"Invalid damage: {damage}", this);
        return;
    }
    if (IsDead) return;

      
    currHp = Mathf.Max (currHp - damage, 0);
    OnDamageTaken?.Invoke(damage);
    OnHealthChanged?.Invoke(currHp, maxHp);
    if (currHp == 0)
      OnDeath?.Invoke();
  }

  public void Heal(int heal)
  {
    if (heal <= 0)
    {
        Debug.LogWarning($"Invalid heal: {heal}", this);
        return;
    }
    if (IsDead || currHp == maxHp) return;

    currHp = Mathf.Min (currHp + heal, maxHp);
    OnHealthChanged?.Invoke(currHp, maxHp);
  }
}
