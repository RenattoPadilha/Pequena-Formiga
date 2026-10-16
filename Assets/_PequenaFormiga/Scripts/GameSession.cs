using UnityEngine;

public static class GameSession
{
  public static int LastLevel { get; private set; } = -1;

  public static void SetLastLevel(int newLevel)
  {
    if (newLevel < 0)
    {
      Debug.LogWarning($"Invalid level: {newLevel}");
      return;
    }
    LastLevel = newLevel;
    Debug.Log(newLevel);
  }
}
