using UnityEngine;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile {
    
    public Vector3 WorldPosition;
    public Position GridPosition;
    
    public Tile(Position position, Vector3 worldPosition) {
      GridPosition = position;
      WorldPosition = worldPosition;
    }
  }
}