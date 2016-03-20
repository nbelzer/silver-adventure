using UnityEngine;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile {
    
    public Vector3 WorldPosition;
    public Position GridPosition;
    public GameObject TileObject;
    public float height;
    
    public Tile(Position position, Vector3 worldPosition, float height, GameObject tileObject) {
      this.GridPosition = position;
      this.WorldPosition = worldPosition;
      this.height = height;
      this.TileObject = tileObject;
      
      // tileObject.transform.localScale = new Vector3(1, 1 * height, 1);
      var renderer = tileObject.GetComponentInChildren<MeshRenderer>();
      
      if (height < 0.3) {
        renderer.material.color = Color.Lerp(new Color(39/255f,86/255f,107/255f), new Color(81/255f, 175/255f, 188/255f), height/0.3f);
      } else if (height < 0.8) {
        renderer.material.color = Color.Lerp(new Color(79/255f,141/255f,53/255f), new Color(35/255f, 78/255f, 17/255f), (height-0.3f)/0.5f);
      } else if (height <= 1.0) {
        renderer.material.color = Color.Lerp(new Color(96/255f, 51/255f, 27/255f), new Color(92/255f, 74/255f, 64/255f), (height-0.8f)/0.2f);
      }
    }
  }
}