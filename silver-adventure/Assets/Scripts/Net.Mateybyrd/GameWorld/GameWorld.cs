using System;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  public class GameWorld {
   
    public GridManager Grid;
    
    public GameWorld() {
      Grid = new GridManager();
    }
  }
}