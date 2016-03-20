using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  public class GameWorld : MonoBehaviour {
   
    public GridManager Grid;
    public MapGenerator Generator;
    
    public int MapSize;
    public int PoolSize; 
    public AnimationCurve Height;
    
    public GameObject TilePrefab;
    
    void Start() {
      Grid = new GridManager();
      PoolManager.instance.CreatePool(TilePrefab, PoolSize);
      GenerateWorld();
    }

    public void GenerateWorld() {
      Generator = FindObjectOfType<MapGenerator>();
      Generator.MapWidth = 2 * MapSize + 1;
      Generator.MapHeight = 2* MapSize + 1;
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
        foreach (var tile in Grid.GetGrid()) {
          tile.Value.TileObject.SetActive(false);
        }
      }
      Grid = new GridManager();
    }
    
    public void CreateTile(AxialPosition pos, float height) {
      var tile = PoolManager.instance.ReuseObject(TilePrefab,
        pos.GetWorldPosition() + Vector3.up * Height.Evaluate(height),
        Quaternion.identity).gameObject;
      Grid.AddTile(new Tile(pos, pos.GetWorldPosition(), height, tile));
    }
  }
}