using UnityEngine;

namespace Net.Mateybyrd.GameWorld {
  public class MapDisplay : MonoBehaviour {

    public Renderer TextureRenderer;

    public void DrawNoiseMap(float[,] noiseMap) {
      var width = noiseMap.GetLength(0);
      var height = noiseMap.GetLength(1);

      var texture = new Texture2D(width, height);
      
      var colourMap = new Color[width * height];
      for (var y = 0; y < height; y++) {
        for (var x = 0; x < width; x++) {
          colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
        }
      }
      
      texture.SetPixels(colourMap);
      texture.Apply();
      
      TextureRenderer.sharedMaterial.mainTexture = texture;
      TextureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
  }
}