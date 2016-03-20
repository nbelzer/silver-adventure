using UnityEngine;
using Net.Mateybyrd;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile {
    
    public Vector3 WorldPosition;
    public Position GridPosition;
    public PoolManager.ObjectInstance TileObject;
    public float height;
    
    public Tile(Position position, Vector3 worldPosition, float height, PoolManager.ObjectInstance tileObject) {
      this.GridPosition = position;
      this.WorldPosition = worldPosition;
      this.height = height;
      this.TileObject = tileObject;
    }
  }
}