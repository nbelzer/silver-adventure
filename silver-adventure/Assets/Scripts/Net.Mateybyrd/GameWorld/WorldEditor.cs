using System.Collections.Generic;
using UnityEngine;
using Net.Mateybyrd.GameWorld.Grid;

namespace Net.Mateybyrd.GameWorld {
  public class WorldEditor : MonoBehaviour {
    public enum EditMode {
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
      
      SelectMode(mouseTile);
      switch (editMode) {
        case EditMode.Move:
          MoveTerrain(mouseTile);
          break;
        default:
          Debug.LogError("Editmode not recognized");
          break;
      }
    }
    
    private void CheckModes() {
      if (Input.GetKeyDown(KeyCode.E)) editMode = EditMode.Move;
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
    
    private void SmoothSelectedTerrain() {
      foreach (Tile tile in selectedTiles) {
        var height = 0f;
        var count = 0;
        foreach (Position neigh in tile.GridPosition.GetNeighbours()) {
          var neighbour = GameWorld.Grid.GetTileAt(neigh);
          if (neighbour != null) {
            height += neighbour.TileObject.transform.position.y;
            count++;
          }
        }
        var average = Mathf.Round( (height / count) / stepSize)  * stepSize; 
        tile.TileObject.transform.position += Vector3.up * (average - tile.TileObject.transform.position.y);
      }
    }
    
    private void MoveTerrain(Tile t) {
      if (Input.GetKeyDown(KeyCode.Q)) {
        MoveTiles(Vector3.up * stepSize);
      } else if (Input.GetKeyDown(KeyCode.A)) {
        MoveTiles(-Vector3.up * stepSize);
      }
      
      
      if (Input.GetKeyDown(KeyCode.S)) {
        SmoothSelectedTerrain();
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
        foreach (CubePosition c in mouseTile.GridPosition.GetInRange(toolRadius)) {
          var t = GameWorld.Grid.GetTileAt(c);
          if (t != null) {
            Gizmos.color = new Color(0.09f, .757f, .647f); // Teal
            DrawCubeAtTile(t, 1.5f);
          }
        }
      }
      
      foreach (Tile t in selectedTiles) {
        Gizmos.color = new Color(.153f, .392f, .773f); // Blue
        DrawCubeAtTile(t, 0.8f);
      }
      
      if (mouseTile != null) {
        Gizmos.color = new Color(1f, .475f, .118f); // Orange
        DrawCubeAtTile(mouseTile, 1.5f);
      }
    }
    
    private void DrawCubeAtTile(Tile t, float scale) {
      Gizmos.DrawCube(t.GridPosition.GetWorldPosition() + Vector3.up * (5 + t.TileObject.transform.position.y), Vector3.one * 0.5f * scale);
    }
  }
}