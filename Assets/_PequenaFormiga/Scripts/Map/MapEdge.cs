using UnityEngine;
[System.Serializable]

public struct MapEdge
{
  [SerializeField] private MapDirection direction; 
  [SerializeField] private MapNode target;

  public MapDirection Direction => direction;
  public MapNode Target => target;

  public MapEdge(MapDirection direction, MapNode target)
    {
        this.direction = direction;
        this.target = target;
    }
}
