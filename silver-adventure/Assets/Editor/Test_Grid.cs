using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

using Net.Mateybyrd.GameWorld;
using Net.Mateybyrd.GameWorld.Grid;

public class Test_GridGenerator {
  
  private GameWorld _world;
  
  [Test]
  public void GenerateGrid() {
    _world = new GameWorld();
    CreateSampleWorld(5);
  }
  
  [Test]
  public void GetNeighbours() {
    foreach (var item in _world.Grid.GetTileAt(new CubePosition(1, 1, -2)).GridPosition.GetNeighbours()) {
      if (item.GetType() != typeof(CubePosition)) continue;
      var cube = (CubePosition) item;
    }
  }
  
  public void CreateSampleWorld(int gridSize) {
    for (var x = -gridSize; x <= gridSize; x++) {
      for (var z = -gridSize; z <= gridSize; z++) {
        var y = -x-z;

        if (y >= -gridSize && y <= gridSize) {
          _world.Grid.AddTile(new Tile(new CubePosition(x, y ,z), Vector3.zero, 0, null));
        }
      }
    }
  }
}
