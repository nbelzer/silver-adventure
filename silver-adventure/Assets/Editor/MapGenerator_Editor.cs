using UnityEditor;
using UnityEngine;
using Net.Mateybyrd.GameWorld;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
  
  public override void OnInspectorGUI() {
    var mapGen = (MapGenerator) target;
    
    if (DrawDefaultInspector()) {
      if (mapGen.autoUpdate) {
        mapGen.GenerateMap();
      }
    }
    
    if (GUILayout.Button("Generate")) {
      mapGen.GenerateMap();
      var world = FindObjectOfType<GameWorld>();
      world.ResetWorld();
      world.GenerateWorld(5);
    }
  }
}