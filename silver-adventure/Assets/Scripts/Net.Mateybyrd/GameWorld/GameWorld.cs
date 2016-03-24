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
    public Material WaterMaterial;
    public Gradient Terrain;
    public Material LandMaterial;
    
    public bool Animation;
    
    void Start() {
      Grid = new GridManager();
      PoolManager.instance.CreatePool(TilePrefab, PoolSize);
      GenerateWorld();
      StartCoroutine(Animate());
      
    }

    private IEnumerator Animate() {
      while (true)
      {
        if (Animation) {
          Generator.Offset.x += 0.1f * Time.deltaTime;
          Generator.Offset.y += 0.2f * Time.deltaTime;
          UpdateWorld();
        }
        yield return new WaitForEndOfFrame();
      }
    }
    
    public void GenerateWorld() {
      Generator = FindObjectOfType<MapGenerator>();
      UpdateWorld();
    }
    
    public void UpdateWorld() {
      Generator.MapWidth = 2 * MapSize + 1;
      Generator.MapHeight = 2* MapSize + 1;
      
      var heightMap = Generator.GenerateMap();
      var grid = Grid.GetGrid();
      
      for (var q = -MapSize; q <= MapSize; q++) {
        for (var r = -MapSize; r <= MapSize; r++) {
          
          var z = -q-r;
          if (z >= -MapSize && z <= MapSize) {
            var position = new AxialPosition(q, r);
            
            if (grid.ContainsKey(position)) {
              UpdateTile(grid[position], heightMap[q+MapSize, r+MapSize]);
            } else {
              CreateTile(position, heightMap[q+MapSize,r+MapSize]);
            }
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
        pos.GetWorldPosition() + Vector3.up * Height.Evaluate(height),
        Quaternion.identity).gameObject;
      var tile = new Tile(pos, pos.GetWorldPosition(), height, tileObject);
      SetTileColor(tile);
      Grid.AddTile(tile);
    }
    
    private void UpdateTile(Tile t, float height) {
      t.Height = height;
      SetTileColor(t);
      SetTileHeight(t);
    }

    private void SetTileColor(Tile t) {
      var renderer = t.TileObject.GetComponentInChildren<MeshRenderer>();
      var mesh = t.TileObject.GetComponentInChildren<MeshFilter>();
      if (t.Height < WaterLevel)
      {
        var percentage = Mathf.Clamp(t.Height / WaterLevel, 0.01f, 0.99f);
        renderer.material = WaterMaterial;
        var newUvs = new Vector2[mesh.mesh.vertices.Length];
        for (var i = 0; i < newUvs.Length; i++) {
          newUvs[i] = new Vector2(percentage, 0);
        }
        mesh.mesh.uv = newUvs;
      } else
      {
        var percentage = Mathf.Clamp((t.Height - WaterLevel) / (1-WaterLevel), 0.01f, 0.99f);
        renderer.material = LandMaterial;
        var newUvs = new Vector2[mesh.mesh.vertices.Length];
        for (var i = 0; i < newUvs.Length; i++) {
          newUvs[i] = new Vector2(percentage, 0);
        }
        mesh.mesh.uv = newUvs;
      }
    }
    
    private void SetTileHeight(Tile t) {
      var pos = t.TileObject.transform.position;
      pos.y = Height.Evaluate(t.Height);
      t.TileObject.transform.position = pos;
    }    
  }
}