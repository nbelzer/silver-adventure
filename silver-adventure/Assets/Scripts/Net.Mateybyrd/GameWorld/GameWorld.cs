using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  
  [Serializable]
  public struct WorldSave {
    public float[,] heightMap;
    public float[,] objectMap;
  }
  
  public class GameWorld : MonoBehaviour {
   
    public static GridManager Grid = new GridManager();
    public MapGenerator Generator;
    
    // Map properties
    public int MapSize;
    public AnimationCurve Height;
    public AnimationCurve reverseHeight;
    
    public GameObject TilePrefab;
    public float WaterLevel;
    public Material WaterMaterial;
    public Material LandMaterial;
    
    private float[,] heightMap;
    private WorldSave save;
    
    void Start() {
      Height.MoveKey(1, new Keyframe(Mathf.Clamp(WaterLevel, Height.keys[0].time + 0.01f, 1f), Height.keys[1].value));
      
      // Setup reverseheight animation curve.
      reverseHeight = new AnimationCurve();
      var animation = new AnimationUtility();
      for (var i = 0; i < Height.keys.Length; i++) {
        var key = Height.keys[i];
        reverseHeight.AddKey(key.value, key.time);
      }
      for (var i = 0; i < reverseHeight.keys.Length; i++) { 
        AnimationUtility.SetKeyLeftTangentMode(Height, i, AnimationUtility.TangentMode.Linear);   
        AnimationUtility.SetKeyRightTangentMode(Height, i, AnimationUtility.TangentMode.Linear);     
        AnimationUtility.SetKeyLeftTangentMode(reverseHeight, i, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(reverseHeight, i, AnimationUtility.TangentMode.Linear);
      }
    }
    
    // Update world so that when changes are applied to tile heights the game will update the colour of that tile.
    void Update() {
      if (Grid.GetGrid().Count > 0) UpdateWorld();
    }
    
    // Generate a map, if fromheightmap == true it will use the heightmap variable defined above.
    public void GenerateWorld(bool fromHeightMap) {
      Generator = FindObjectOfType<MapGenerator>();
      
      if (fromHeightMap) {
        LoopTroughMap(CreateTile);       
      } else {
        CreateFlatWorld();
      }
    }
    
    // Create a world from the generator.
    public void CreateWorld() {
      Generator.MapWidth = 2 * MapSize + 1;
      Generator.MapHeight = 2* MapSize + 1;
      
      heightMap = Generator.GenerateMap();
    }
    
    // Create a flat world
    public void CreateFlatWorld() {
      heightMap = new float[2 * MapSize + 1, 2 * MapSize + 1];
      LoopTroughMap(CreateTile);
    }
    
    // Update the entire world by looping through each tile.
    public void UpdateWorld() {
      LoopTroughMap(UpdateTile);
    }
    
    // Save the map to a binary file.
    public void SaveMap() {
      if (!Directory.Exists("Saves"))
        Directory.CreateDirectory("Saves");
            
      var formatter = new BinaryFormatter();
      var saveFile = File.Create("Saves/save.map");
      
      
      save = new WorldSave();
      save.heightMap = new float[MapSize * 2 + 1, MapSize * 2 + 1];
      save.objectMap = new float[MapSize * 2 + 1, MapSize * 2 + 1];
      
      LoopTroughMap(SaveTileToMap);
      
 
      formatter.Serialize(saveFile, save);
      saveFile.Close();
    }
    
    // Load the map from a binary file.
    public void LoadMap() {
      if (!Directory.Exists("Saves"))
        Debug.LogError("No save directory found");
      
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream saveFile = File.Open("Saves/save.map", FileMode.Open);

      save = (WorldSave)formatter.Deserialize(saveFile);
      saveFile.Close();
      
      heightMap = save.heightMap;
      GenerateWorld(true);
    }
    
    // Save a single tile to the save.
    public void SaveTileToMap(Position p) {
      var axial = p as AxialPosition;
      
      save.heightMap[(int)axial.Q + MapSize, (int)axial.R + MapSize] = Grid.GetTileAt(p).Height;
    }
    
    // Method used to perform an action on each tile in the map.
    private void LoopTroughMap(Action<Position> performAction) {
      var grid = Grid.GetGrid();
      
      for (var q = -MapSize; q <= MapSize; q++) {
        for (var r = -MapSize; r <= MapSize; r++) {
          var z = -q-r;
          if (z >= -MapSize && z <= MapSize) {
            var position = new AxialPosition(q, r);
            
            performAction(position);  
          }
        }
      }
    }
    
    // Method that can be given to the LoopTroughMap function to update a tile.
    private void UpdateTile(Position p) {
      if (Grid.GetTileAt(p) != null) {
        UpdateTile(Grid.GetTileAt(p));
      }
    }
    
    // Method that can be given to the LoopTroughMap function to create a tile.
    private void CreateTile(Position p) {
      var axial = p as AxialPosition;
      CreateTile(axial, heightMap[(int)(axial.Q) + MapSize, (int)(axial.R) + MapSize]);
    }
    
    // Perform random alterations of the terrain to give a more randomized look to the terrain. Alterations are of scale 0.05f.
    public void RandomAlterations() {
      var grid = Grid.GetGrid();
      foreach (var item in grid) {
        item.Value.TileObject.transform.position += Vector3.up * UnityEngine.Random.Range(-1, 2) * 0.05f;
        var height = item.Value.TileObject.transform.position.y;
        if (height < 0) item.Value.TileObject.transform.position += Vector3.up * -height;
        if (height > 6) item.Value.TileObject.transform.position += Vector3.up * (6 - height);
      }
    }
    
    // Remove the entire world saved in the grid.
    public void ResetWorld() {
      if (Grid != null) {
        foreach (var tile in Grid.GetGrid()) {
          DestroyImmediate(tile.Value.TileObject);
        }
      }
      Grid = new GridManager();
    }
    
    // Create a tile, set it's color and add it to the grid.
    public void CreateTile(AxialPosition pos, float height) {
      var tileObject = Instantiate(TilePrefab,
        pos.GetWorldPosition() + Vector3.up * Mathf.Round(Height.Evaluate(height)/0.05f) * 0.05f,
        Quaternion.identity) as GameObject;
      var tile = new Tile(pos, pos.GetWorldPosition(), height, tileObject);
      tileObject.transform.SetParent(this.transform);
      SetTileColor(tile);
      Grid.AddTile(tile);
    }
    
    // Update a tile's height and color.
    private void UpdateTile(Tile t) {
      var height = UpdateTileHeight(t);
      t.Height = reverseHeight.Evaluate(height);
      SetTileColor(t);
    }
    
    // Update a tile's height based on the transform of that tile.
    private float UpdateTileHeight(Tile t) {
      var meshHeight = t.TileObject.transform.GetChild(0).transform.localPosition.y;
      if (meshHeight != 0) {
        t.TileObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
        t.TileObject.transform.position += Vector3.up * meshHeight;
      }
      return t.TileObject.transform.position.y;
    }

    // Set a tile's color based on the tile's height.
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
  }
}