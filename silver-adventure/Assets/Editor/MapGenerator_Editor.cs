using UnityEditor;
using UnityEngine;
using Net.Mateybyrd.GameWorld;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
  
  public override void OnInspectorGUI() {

    var style = new GUIStyle(GUI.skin.box)
    {
      margin = new RectOffset(0, 0, 0, 0),
      padding = new RectOffset(0, 0, 0, 0)
    };

    var mapGen = (MapGenerator) target;

    // if (DrawDefaultInspector()) {
    //   if (mapGen.AutoUpdate) {
    //     mapGen.GenerateMap();
    //     var world = FindObjectOfType<GameWorld>();
    //     world.UpdateWorld();
    //   }
    // }

    if (GUILayout.Button("Generate")) {
      mapGen.GenerateMap();
      var world = FindObjectOfType<GameWorld>();
      world.ResetWorld();
      world.GenerateWorld(true);
    }
  }
}