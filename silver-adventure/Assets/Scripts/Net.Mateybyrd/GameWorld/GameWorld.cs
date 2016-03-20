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
      PoolManager.instance.CreatePool(tilePrefab, 500);
      GenerateWorld(10);
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
          tile.Value.TileObject.Destroy();
        }
      }
      Grid = new GridManager();
    }
    
    public void CreateTile(AxialPosition pos, float height) {
      var tile = PoolManager.instance.ReuseObject(tilePrefab,  pos.GetWorldPosition() + Vector3.up * height * 10, Quaternion.identity);
      // var tile = GameObject.Instantiate(tilePrefab,, Quaternion.identity) as GameObject;
      // tile.transform.SetParent(this.transform);
      Grid.AddTile(new Tile(pos, pos.GetWorldPosition(), height, tile));
    }
  }
}