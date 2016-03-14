using System;
using System.Collections.Generic;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  /// <summary>
  ///   This class contains a grid manager that can be given any type of tile   to manage
  /// </summary>
  public class GridManager<T> {

    // The actual grid stored in a dictionary.
    private readonly Dictionary<T, Tile<T>> _tiles = new Dictionary<T, Tile<T>>();

    /// <summary>
    /// Adds a tile to the grid, if the item could not be added an error will be displayed
    /// </summary>
    public void AddTile(Tile<T> tile) {
      try {
        _tiles.Add(tile.GridPosition, tile);
      } catch (Exception e) {
        UnityEngine.Debug.LogError("Could not add the tile (" + tile.GridPosition + ")you wanted: " + e);
      }
    }

    /// <summary>
    /// Returns a tile of the type that was given to this GridManager instance.
    /// If the item could not be found an error will be displayed.
    /// </summary>
    public Tile<T> GetTileAt(T position) {
      try {
        return _tiles[position];
      } catch (Exception e) {
        UnityEngine.Debug.LogError("Could not find tile you were looking for: " + e);
        return null;
      }
    }
    
    public Dictionary<T, Tile<T>> GetGrid() {
      return _tiles;
    }
  }
}