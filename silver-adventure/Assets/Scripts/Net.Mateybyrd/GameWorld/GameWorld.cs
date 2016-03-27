using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  public class GameWorld : MonoBehaviour {
   
    public static GridManager Grid = new GridManager();
    // public MapGenerator Generator;
    
    public int MapSize;
    public int PoolSize; 
    public AnimationCurve Height;
    
    public GameObject TilePrefab;
    public float WaterLevel;
    public Material WaterMaterial;
    public Material LandMaterial;
    
    public bool Animation;
    
    void Update() {
      UpdateWorld();
    }
    
    public void GenerateWorld() {
      // Generator = FindObjectOfType<MapGenerator>();
      UpdateWorld();
    }
    
    public void UpdateWorld() {
      // Generator.MapWidth = 2 * MapSize + 1;
      // Generator.MapHeight = 2* MapSize + 1;
      // Height.MoveKey(1, new Keyframe(Mathf.Clamp(WaterLevel, Height.keys[0].time + 0.01f, 1f), Height.keys[1].value));
      
      // var heightMap = Generator.GenerateMap();
      var grid = Grid.GetGrid();
      
      for (var q = -MapSize; q <= MapSize; q++) {
        for (var r = -MapSize; r <= MapSize; r++) {
          
          var z = -q-r;
          if (z >= -MapSize && z <= MapSize) {
            var position = new AxialPosition(q, r);
            
            if (grid.ContainsKey(position)) {
              UpdateTile(grid[position]);
            } else {
              CreateTile(position, 0);
            }
            
          }
        }
      }
    }
    
    public void RandomAlterations() {
      var grid = Grid.GetGrid();
      foreach (var item in grid) {
        item.Value.TileObject.transform.position += Vector3.up * UnityEngine.Random.Range(-0.05f, 0.05f);
        var height = item.Value.TileObject.transform.position.y;
        if (height < 0) item.Value.TileObject.transform.position += Vector3.up * -height;
        if (height > 6) item.Value.TileObject.transform.position += Vector3.up * (6 - height);
      }
    }
    
    public void ResetWorld() {
      if (Grid != null) {
        foreach (var tile in Grid.GetGrid()) {
          DestroyImmediate(tile.Value.TileObject);
        }
      }
      Grid = new GridManager();
    }
    
    public void CreateTile(AxialPosition pos, float height) {
      var tileObject = Instantiate(TilePrefab,
        pos.GetWorldPosition() + Vector3.up * Height.Evaluate(height),
        Quaternion.identity) as GameObject;
      var tile = new Tile(pos, pos.GetWorldPosition(), height, tileObject);
      tileObject.transform.SetParent(this.transform);
      SetTileColor(tile);
      Grid.AddTile(tile);
    }
    
    private void UpdateTile(Tile t) {
      var height = UpdateTileHeight(t);
      t.Height = Height.Evaluate(height);
      SetTileColor(t);
      // SetTileHeight(t);
    }
    
    private float UpdateTileHeight(Tile t) {
      var meshHeight = t.TileObject.transform.GetChild(0).transform.localPosition.y;
      if (meshHeight != 0) {
        t.TileObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
        t.TileObject.transform.position += Vector3.up * meshHeight;
      }
      return t.TileObject.transform.position.y;
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