using System.Collections.Generic;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  public class GameWorld : MonoBehaviour {
   
    public GridManager Grid;
    public MapGenerator Generator;
    
    public int MapSize;
    public int PoolSize; 
    public AnimationCurve Height;
    
    public GameObject tilePrefab;
    
    void Start() {
      Grid = new GridManager();
      PoolManager.instance.CreatePool(tilePrefab, PoolSize);
      GenerateWorld();
    }

    public void GenerateWorld() {
      Generator = FindObjectOfType<MapGenerator>();
      Generator.mapWidth = 2 * MapSize + 1;
      Generator.mapHeight = 2* MapSize + 1;
      var heightMap = Generator.GenerateMap();
      
      for (var q = -MapSize; q <= MapSize; q++) {
        for (var r = -MapSize; r <= MapSize; r++) {
          
          var z = -q-r;
          if (z >= -MapSize && z <= MapSize) {
            CreateTile(new AxialPosition(q, r), heightMap[q+MapSize,r+MapSize]);
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
      var tile = PoolManager.instance.ReuseObject(tilePrefab,  pos.GetWorldPosition() + Vector3.up * Height.Evaluate(height), Quaternion.identity);
      // var tile = GameObject.Instantiate(tilePrefab,, Quaternion.identity) as GameObject;
      // tile.transform.SetParent(this.transform);
      Grid.AddTile(new Tile(pos, pos.GetWorldPosition(), height, tile));
    }
  }
}