using UnityEngine;
using UnityEditor;
using System.Collections;
using Net.Mateybyrd.GameWorld;

public class MapEditor : EditorWindow {
  
  Vector2 mapSize;
  GameWorld world;

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
    
    EditorGUI.DrawRect(new Rect(0, 0, 400, 40), Color.gray);
    EditorGUILayout.Space();
    GUILayout.Label("  Map Editor", EditorStyles.largeLabel);
    EditorGUILayout.Space();
    
    EditorGUI.indentLevel++;
      GUILayout.Label("Base settings", EditorStyles.boldLabel);
        mapSize = EditorGUILayout.Vector2Field("Map Size", mapSize);
    EditorGUI.indentLevel--;
    
    if (GUILayout.Button("Generate map")) {
      GenerateFlatTerrain(mapSize);
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
  
  void GenerateFlatTerrain(Vector2 mapSize) {
    world.GenerateWorld();
  }
}
