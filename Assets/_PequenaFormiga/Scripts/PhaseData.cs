using UnityEngine;

[CreateAssetMenu(fileName = "PhaseData", menuName = "Scriptable Objects/PhaseData")]
public class PhaseData : ScriptableObject
{
  [SerializeField] private string phaseName;
  [SerializeField] [Min(0)] private int phaseNumber = -1;
  [SerializeField] private string sceneName;

  public string PhaseName => phaseName;
  public int PhaseNumber => phaseNumber;
  public string SceneName => sceneName;

  public bool IsValid => (!string.IsNullOrEmpty(phaseName)) && (phaseNumber >= 0) && (!string.IsNullOrEmpty(sceneName));

  void OnValidate()
  {
    if (string.IsNullOrEmpty(phaseName)) Debug.LogWarning("Phase Name not assigned", this);
    if (phaseNumber < 0) Debug.LogWarning("Phase Number not assigned", this);
    if (string.IsNullOrEmpty(sceneName)) Debug.LogWarning("Scene Name not assigned", this);
  }
}
