using UnityEngine;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile<T> {
    
    public Vector3 WorldPosition;
    public T GridPosition;
    
    public Tile() {
      var grid = new GridPosition(5,5);
      var neighbours = grid.GetNeighbours();
    }
  }
}