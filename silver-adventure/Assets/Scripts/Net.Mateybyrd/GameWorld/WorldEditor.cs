using System.Collections.Generic;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  public class WorldEditor : MonoBehaviour {
    
    public LayerMask TileLayer;
    public float stepSize;
    
    private List<Tile> selectedTiles = new List<Tile>();
    
    void Update() {
      var tile = GetTileFromRay();
      if (tile != null) {
        if (Input.GetMouseButton(0)) {
          if (!selectedTiles.Contains(tile)) {
            selectedTiles.Add(tile);
          }
        }
        if (Input.GetMouseButton(1)) {
          selectedTiles.Remove(tile);
        }
      }
      if (Input.GetKeyDown(KeyCode.D)) {
        selectedTiles.Clear();
      }
      
      if (Input.GetKeyDown(KeyCode.Q)) {
        MoveTiles(Vector3.up * stepSize);
      }
      if (Input.GetKeyDown(KeyCode.A)) {
        MoveTiles(-Vector3.up * stepSize);
      }
    }
    
    private void MoveTiles(Vector3 adjustment) {
      foreach (Tile t in selectedTiles) {
        t.TileObject.transform.position += adjustment;
      }
    }
    
    private Tile GetTileFromRay() {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 50f, TileLayer)) {
      Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 2f);
        var axial = new AxialPosition(hit.point);
        
        return GameWorld.Grid.GetTileAt(axial);
      } else {
        return null;
      }
    }
    
    void OnDrawGizmos() {
      foreach (Tile t in selectedTiles) {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(t.GridPosition.GetWorldPosition() + Vector3.up * (5 + t.TileObject.transform.position.y), Vector3.one);
      }
    }
  }
}