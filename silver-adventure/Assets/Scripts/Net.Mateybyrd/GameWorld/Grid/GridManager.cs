using System;
using System.Collections.Generic;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  /// <summary>
  ///   This class contains a grid manager that can be given any type of tile   to manage
  /// </summary>
  public class GridManager<T, TK> where T : Tile<TK> {

    // The actual grid stored in a dictionary.
    private readonly Dictionary<TK, T> _tiles = new Dictionary<TK, T>();

    /// <summary>
    /// Adds a tile to the grid, if the item could not be added an error will be displayed
    /// </summary>
    public void AddTile(T tile) {
      try {
        _tiles.Add(tile.GridPosition, tile);
      } catch (Exception e) {
        UnityEngine.Debug.LogError("Could not add the tile you wanted: " + e);
      }
    }

    /// <summary>
    /// Returns a tile of the type that was given to this GridManager instance.
    /// If the item could not be found an error will be displayed.
    /// </summary>
    public T GetTileAt(TK position) {
      try {
        return _tiles[position];
      } catch (Exception e) {
        UnityEngine.Debug.LogError("Could not find tile you were looking for: " + e);
        return null;
      }
    }
  }
}