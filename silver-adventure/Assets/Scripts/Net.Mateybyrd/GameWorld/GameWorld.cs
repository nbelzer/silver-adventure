using System;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  public class GameWorld<T> {
   
    public GridManager<T> Grid;
    
    public GameWorld() {
      Grid = new GridManager<T>();
    }
  }
}