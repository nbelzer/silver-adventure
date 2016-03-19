using UnityEngine;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile {
    
    public Vector3 WorldPosition;
    public Position GridPosition;
    public GameObject TileObject;
    public float height;
    
    public Tile(Position position, Vector3 worldPosition, float height, GameObject tileObject) {
      this.GridPosition = position;
      this.WorldPosition = worldPosition;
      this.height = height;
      this.TileObject = tileObject;
    }
  }
}