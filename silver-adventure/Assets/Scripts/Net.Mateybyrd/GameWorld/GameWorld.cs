using System;
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
    public float WaterLevel;
    public Gradient Water;
    public Gradient Terrain;
    
    public bool Animation;
    
    void Start() {
      Grid = new GridManager();
      PoolManager.instance.CreatePool(TilePrefab, PoolSize);
      GenerateWorld();
      if (Animation) StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
      while (true)
      {
        Generator.Offset.x += 0.1f * Time.deltaTime;
        Generator.Offset.y += 0.2f * Time.deltaTime;
        ResetWorld();
        GenerateWorld();
        yield return null;
      }
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
      var tileObject = PoolManager.instance.ReuseObject(TilePrefab,
        pos.GetWorldPosition(),
        TilePrefab.transform.rotation).gameObject;
      var tile = new Tile(pos, pos.GetWorldPosition(), height, tileObject);
      SetTileColor(tile);
      Grid.AddTile(tile);
    }

    private void SetTileColor(Tile t) {
      if (t.Height < WaterLevel) {
        t.TileObject.GetComponent<SpriteRenderer>().color = Water.Evaluate(t.Height/WaterLevel);
      } else {
        t.TileObject.GetComponent<SpriteRenderer>().color = Terrain.Evaluate((t.Height-WaterLevel)/(1-WaterLevel));
      }
    }
  }
}