using UnityEngine;
using UnityEditor;
using System.Collections;
using Net.Mateybyrd.GameWorld;

public class MapEditor : EditorWindow {
  
  MapGenerator generator;
  GameWorld world;
  WorldEditor editor;

  [MenuItem ("MateyByrd/MapEditor")]
  static void Init() {
    var window = (MapEditor) EditorWindow.GetWindow(typeof(MapEditor));
    window.Show();
  }
  
  void OnGUI() {
    if (world == null) {
      world = FindObjectOfType<GameWorld>();
      if (world == null) {
        world = new GameWorld();
      }
    }
    if (generator == null) {
      generator = FindObjectOfType<MapGenerator>();
    }
    if (editor == null) {
      editor = FindObjectOfType<WorldEditor>();
    }
    
    EditorGUI.DrawRect(new Rect(0, 0, 400, 40), Color.gray);
    EditorGUILayout.Space();
    GUILayout.Label("  Map Editor", EditorStyles.largeLabel);
    EditorGUILayout.Space();
    
    EditorGUI.indentLevel++;
      GUILayout.Label("Base settings", EditorStyles.boldLabel);
        world.MapSize = EditorGUILayout.IntField("Map Size", world.MapSize);
        generator.MapWidth = world.MapSize * 2 + 1;
        generator.MapHeight = world.MapSize * 2 + 1;
        generator.NoiseScale = EditorGUILayout.FloatField("Noise scale", generator.NoiseScale);
        generator.Octaves = EditorGUILayout.IntField("Octaves", generator.Octaves);
        generator.Persistance = EditorGUILayout.Slider("Persistance", generator.Persistance, 0, 1f);
        generator.Lacunarity = EditorGUILayout.FloatField("Lacunarity", generator.Lacunarity);
        generator.Seed = EditorGUILayout.IntField("Seed", generator.Seed);
        generator.Offset = EditorGUILayout.Vector2Field("Offset", generator.Offset);
    EditorGUI.indentLevel--;
    
    EditorGUI.indentLevel++;
      GUILayout.Label("Editor settings", EditorStyles.boldLabel);
      world.WaterLevel = EditorGUILayout.Slider("Water level", world.WaterLevel, 0, 0.6f);
      editor.stepSize = Mathf.Clamp(Mathf.Round(EditorGUILayout.FloatField("Step size", editor.stepSize)/ 0.05f) * 0.05f, 0.05f, 10f);
      editor.toolRadius = EditorGUILayout.IntSlider("Tool radius", editor.toolRadius, 0, world.MapSize);
    EditorGUI.indentLevel--;
    
    
    EditorGUILayout.Space();
    GUILayout.Label("Generate buttons", EditorStyles.boldLabel);
    
    if (GUILayout.Button("Generate flat map")) {
      GenerateFlatTerrain();
    }
    if (GUILayout.Button("Generate random map")) {
      GenerateRandomTerrain();
    }
    if (GUILayout.Button("Randomization map")) {
      world.RandomAlterations();
      world.UpdateWorld();
    }
    if (GUILayout.Button("Update map")) {
      world.UpdateWorld();
    }
    if (GUILayout.Button("Remove map")) {
      world.ResetWorld();
    }
  }
  
  void GenerateFlatTerrain() {
    world.GenerateWorld(false);
  }
  
  void GenerateRandomTerrain() {
    world.GenerateWorld(true);
  }
}
