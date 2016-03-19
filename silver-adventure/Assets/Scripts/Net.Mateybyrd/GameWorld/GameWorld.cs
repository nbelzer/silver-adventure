using System.Collections.Generic;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  public class GameWorld : MonoBehaviour {
   
    public GridManager Grid;
    public MapGenerator Generator;
    
    public GameObject tilePrefab;
    
    void Start() {
      Grid = new GridManager();
      GenerateWorld(40);
    }

    public void GenerateWorld(int mapSize) {
      Generator = FindObjectOfType<MapGenerator>();
      var heightMap = Generator.GenerateMap();
      
      for (var q = -mapSize; q <= mapSize; q++) {
        for (var r = -mapSize; r <= mapSize; r++) {
          
          var z = -q-r;
          if (z >= -mapSize && z <= mapSize) {
            CreateTile(new AxialPosition(q, r), heightMap[q+mapSize,r+mapSize]);
          }
          
        }
      }
    }
    
    public void ResetWorld() {
      if (Grid != null) {
        foreach (KeyValuePair<Position, Tile> tile in Grid.GetGrid()) {
        DestroyImmediate(tile.Value.TileObject);
        }
      }
      Grid = new GridManager();
    }
    
    public void CreateTile(AxialPosition pos, float height) {
      var tile = GameObject.Instantiate(tilePrefab, pos.GetWorldPosition() + Vector3.up * height * 20, Quaternion.identity) as GameObject;
      tile.transform.SetParent(this.transform);
      Grid.AddTile(new Tile(pos, pos.GetWorldPosition(), height, tile));
    }
  }
}