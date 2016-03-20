using System;
using UnityEngine;

namespace Net.Mateybyrd.GameWorld {
  
  public class MapGenerator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    
    public int seed;
    public Vector2 offset;
    
    public bool autoUpdate;
    
    public float[,] GenerateMap() {
      float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
      
      MapDisplay display = FindObjectOfType<MapDisplay>();
      if (display != null) display.DrawNoiseMap(noiseMap);
      
      return noiseMap;
    }
    
    void OnValidate() {
      if (mapWidth < 1) {
        mapWidth = 1;
      }
      if (mapHeight < 1) {
        mapHeight = 1;
      }
      if (lacunarity < 1) {
        lacunarity = 1;
      }
      if (octaves < 0) {
        octaves = 0;
      }
    }
  }
}