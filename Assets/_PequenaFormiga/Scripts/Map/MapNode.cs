using UnityEngine;
using System.Collections.Generic;

public class MapNode : MonoBehaviour
{
  [SerializeField] private LevelData levelData;
  [SerializeField] private Transform playerAnchor;
  [SerializeField] private Transform tooltipAnchor;
  [SerializeField] private MapEdge[] mapEdges =
    {
      new MapEdge (MapDirection.North, null),
      new MapEdge (MapDirection.East, null),
      new MapEdge (MapDirection.West, null),
      new MapEdge (MapDirection.South, null),
    };

  public LevelData Data => levelData;
  public Transform TooltipAnchor => tooltipAnchor;
  public bool IsValid => ValidateEdges(log: false) && playerAnchor && (!HasLevel || (tooltipAnchor && levelData.IsValid));
  public bool HasLevel => levelData;
  public Vector3 StopPosition => playerAnchor.position;

  void OnValidate()
  {
    if (levelData && !levelData.IsValid) Debug.LogWarning($"Level Data isn't valid\nMapNode Name: {name}", this);
    if (!playerAnchor) Debug.LogWarning($"Player Anchor not assigned\nMapNode Name: {name}", this);
    if (HasLevel && !tooltipAnchor) Debug.LogWarning($"Tooltip Anchor not assigned\nMapNode Name: {name}", this);
  }

  void Awake()
  {
    if (levelData && !levelData.IsValid)
    {
      Debug.LogError("Level Data is invalid or incomplete", this);
    }
    if (!playerAnchor)
    {
      Debug.LogError("Player Anchor Transform not assigned", this);
    }
    if (HasLevel && !tooltipAnchor)
    {
      Debug.LogError("Tooltip Anchor Transform not assigned", this);
    }
    ValidateEdges(log: true);
  }

  public MapNode GetNeighbor(MapDirection direction)
  {
    foreach (var edge in mapEdges)
      if (edge.Direction == direction)
        return edge.Target;
    return null;
  }

  private bool ValidateEdges(bool log)
  {
    bool valid = true;
    HashSet<MapDirection> seen = new HashSet<MapDirection>();

    foreach (var edge in mapEdges)
    {
      if (edge.Target == this)
      {
        valid = false;
        if (log) Debug.LogError($"{edge.Direction} edge points to itself. Node: {name}", this);
      }
      if (!seen.Add(edge.Direction))
      {
        valid = false;
        if (log) Debug.LogError($"Duplicated direction {edge.Direction}. Node: {name}", this);
      }
    }
    return valid;
  }

  void OnDrawGizmos()
  {
    if (!playerAnchor) 
      return;

    Gizmos.color = HasLevel ? Color.yellow : Color.cyan;
    Gizmos.DrawWireSphere(StopPosition, 2f);

    foreach (var edge in mapEdges)
    {
      if (!edge.Target) continue;

      bool returns = edge.Target.GetNeighbor(edge.Direction.Opposite()) == this;
      Gizmos.color = returns ? Color.green : Color.red;
      Gizmos.DrawLine(StopPosition, edge.Target.StopPosition);
    }
  }
}