using System.Collections.Generic;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  public class WorldEditor : MonoBehaviour {
    public enum EditMode {
      Select,
      Move
    } 
    
    public LayerMask TileLayer;
    public EditMode editMode;
    public int toolRadius;
    
    public float stepSize;
    
    private List<Tile> selectedTiles = new List<Tile>();
    
    void Update() {
      var tile = GetTileFromRay();
      
      CheckModes();
      CheckRadius();
      
      if (tile == null) return;
      
      switch (editMode) {
        case EditMode.Select: 
          SelectMode(tile);
          break;
        case EditMode.Move:
          MoveTerrain(tile);
          break;
        default:
          Debug.LogError("Editmode not recognized");
          break;
      }
    }
    
    private void CheckModes() {
      if (Input.GetKeyDown(KeyCode.S)) editMode = EditMode.Select;
      if (Input.GetKeyDown(KeyCode.A)) editMode = EditMode.Move;
    }
    
    private void CheckRadius() {
      if (Input.mouseScrollDelta.x > 0) {
        toolRadius++;
      } else if (Input.mouseScrollDelta.x < 0) {
        toolRadius = Mathf.Max(toolRadius--, 1);;
      }
    }
    
    private void SelectMode(Tile t) {
      if (Input.GetMouseButton(0)) {
        if (!selectedTiles.Contains(t)) {
          selectedTiles.Add(t);
        }
      } else if (Input.GetMouseButton(1)) {
        selectedTiles.Remove(t);
      }
      
      if (Input.GetKeyDown(KeyCode.D)) selectedTiles.Clear();
    }
    
    private void MoveTerrain(Tile t) {
      if (Input.GetMouseButtonDown(0)) {
        MoveTiles(Vector3.up * stepSize);
      } else if (Input.GetMouseButtonDown(1)) {
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