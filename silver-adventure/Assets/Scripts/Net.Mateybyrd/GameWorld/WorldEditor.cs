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
    
    private Tile mouseTile;
    private List<Tile> selectedTiles = new List<Tile>();
    
    void Update() {
      mouseTile = GetTileFromRay();
      
      CheckModes();
      CheckRadius();
      
      if (mouseTile == null) return;
      
      switch (editMode) {
        case EditMode.Select: 
          SelectMode(mouseTile);
          break;
        case EditMode.Move:
          MoveTerrain(mouseTile);
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
      if (Input.mouseScrollDelta.y > 0) {
        toolRadius++;
      } else if (Input.mouseScrollDelta.y < 0) {
        toolRadius = Mathf.Max(toolRadius-1, 0);
      }
    }
    
    private void SelectMode(Tile t) {
      if (Input.GetMouseButton(0)) {
        foreach (CubePosition c in t.GridPosition.GetInRange(toolRadius)) {
          var tile = GameWorld.Grid.GetTileAt(c);
          if (tile == null) continue;
          if (selectedTiles.Contains(tile)) continue;
          selectedTiles.Add(tile);
        }
      } else if (Input.GetMouseButton(1)) {
        foreach (CubePosition c in t.GridPosition.GetInRange(toolRadius)) {
          var tile = GameWorld.Grid.GetTileAt(c);
          if (tile == null) continue;
          selectedTiles.Remove(tile);
        }
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
      // Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 2f);
        var axial = AxialPosition.AxialFromWorld(hit.point);
        
        return GameWorld.Grid.GetTileAt(axial);
      } else {
        return null;
      }
    }
    
    void OnDrawGizmos() {
      if (mouseTile != null) { 
        Gizmos.color = Color.blue;
        foreach (CubePosition c in mouseTile.GridPosition.GetInRange(toolRadius)) {
          var t = GameWorld.Grid.GetTileAt(c);
          if (t != null) {
            DrawCubeAtTile(t);
          }
        }
      }
      
      foreach (Tile t in selectedTiles) {
        Gizmos.color = Color.red;
        DrawCubeAtTile(t);
      }
    }
    
    private void DrawCubeAtTile(Tile t) {
      Gizmos.DrawCube(t.GridPosition.GetWorldPosition() + Vector3.up * (5 + t.TileObject.transform.position.y), Vector3.one);
    }
  }
}