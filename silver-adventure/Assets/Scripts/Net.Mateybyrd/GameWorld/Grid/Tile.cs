using UnityEngine;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile<T> {
    
    public Vector3 WorldPosition;
    public T GridPosition;
    
    public Tile(T position, Vector3 worldPosition) {
      GridPosition = position;
      WorldPosition = worldPosition;
    }
  }
}