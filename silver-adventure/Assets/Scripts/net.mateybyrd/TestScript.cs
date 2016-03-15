using UnityEngine;
using System.Collections.Generic;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd {
  
  public class TestScript : MonoBehaviour {

    public GameWorld.GameWorld World;

    // Use this for initialization
    void Start () {
      World = new GameWorld.GameWorld();
      CreateSampleWorld(5);

      foreach (var item in World.Grid.GetTileAt(new CubePosition(1, 1, -2)).GridPosition.GetNeighbours()) {
        if (item.GetType() != typeof(CubePosition)) continue;
        var cube = (CubePosition) item;
        Debug.Log(cube.ToString());
      }
    }

    // Update is called once per frame
    void Update () { 
        
    }
    
    void OnDrawGizmos() {
      foreach (KeyValuePair<Position, Tile> pair in World.Grid.GetGrid()) {
        Gizmos.DrawCube(pair.Key.GetWorldPosition(), Vector3.one * 0.5f);
      }
    }

    public void CreateSampleWorld(int gridSize) {
      for (var x = -gridSize; x <= gridSize; x++) {
        for (var z = -gridSize; z <= gridSize; z++) {
          var y = -x-z;

          if (y >= -gridSize && y <= gridSize) {
            World.Grid.AddTile(new Tile(new CubePosition(x, y ,z), Vector3.zero));
          }
        }
      }
    }
  }
}

