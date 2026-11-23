public enum MapDirection
{
    North,
    South,
    East,
    West
}

public static class MapDirectionExtensions
{
  public static MapDirection Opposite(this MapDirection direction) => direction switch
  {
    MapDirection.North => MapDirection.South,
    MapDirection.South => MapDirection.North,
    MapDirection.East => MapDirection.West,
    MapDirection.West => MapDirection.East,
    _ => throw new System.ArgumentOutOfRangeException(nameof(direction))
  };
}