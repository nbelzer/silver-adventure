using System;
using System.Collections.Generic;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Temp {
    
    public Temp() {
      var cubeManager = new GridManager<Tile<CubePosition>, CubePosition>();
      var gridManager = new GridManager<Tile<GridPosition>, GridPosition>();
    }
  }
  
  /// <summary>
  ///   This class contains a grid manager that can be given any type of tile   to manage
  /// </summary>
  public class GridManager<T, Type> where T : Tile<Type> { 
    
    private Dictionary<Type, T> _tiles = new Dictionary<Type, T>();
    
    public void AddTile(T tile) {
      try {
        _tiles.Add(tile.gridPosition, tile);
      } catch (Exception e) {
        UnityEngine.Debug.LogError("Could not add the tile you wanted: " + e);
      }
    }
    
    public T GetTileAt(Type position) {
      try {
        return _tiles[position];
      } catch (Exception e) {
        UnityEngine.Debug.LogError("Could not find tile you were looking for: " + e);
        return null;
      }
    }
  }
}