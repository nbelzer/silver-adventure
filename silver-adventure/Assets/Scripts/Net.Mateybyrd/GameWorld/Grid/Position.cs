using System;
using UnityEngine;
using System.Linq;

namespace Net.Mateybyrd.GameWorld.Grid
{
  /// <summary>
  /// A Position<T> class should implement the given methods and a way of storing the position.
  /// </summary>
  public abstract class Position
  {
    public static readonly float TileSize = 1f;
    /// <summary>
    /// Returns all the neighbours of this coordinate.
    /// </summary>
    public abstract Position[] GetNeighbours();

    public abstract Vector3 GetWorldPosition();
  }

  public class GridPosition: Position, IEquatable<GridPosition> {

    // Store position
    public int XPos, YPos;

    // Neighbouring tile offsets
    private static readonly GridPosition[] Neighbours = {
      new GridPosition(-1,  0), new GridPosition( 1,  0),
      new GridPosition( 0,  1), new GridPosition( 0, -1)
    };

    public GridPosition(int x, int y) {
      XPos = x;
      YPos = y;
    }
    
    public GridPosition(Vector3 worldPosition) {
      XPos = (int) (worldPosition.x / TileSize);
      YPos = (int) (worldPosition.y / TileSize);
    }

    public override Vector3 GetWorldPosition() {
      return new Vector3(TileSize * XPos, 0, TileSize * YPos);
    }

    public override Position[] GetNeighbours() {
      return Neighbours.Select(neighbour => new GridPosition(XPos + neighbour.XPos, YPos + neighbour.YPos)).ToArray();
    }

    public bool Equals(GridPosition pos) {
      return (XPos == pos.XPos && YPos == pos.YPos);
    }

    public override bool Equals(object o) {
      return Equals(o as GridPosition);
    }

    public override int GetHashCode() {
      unchecked {
        return (XPos*397) ^ YPos;
      }
    }

    public override string ToString() {
      return "Grid [ " + XPos + ", " + YPos + "]";
    }
  }

  public class CubePosition: Position, IEquatable<CubePosition> {

    // Store position
    public int XPos, YPos, ZPos;

    private static readonly CubePosition[] Neighbours = {
      new CubePosition(-1,  0,  1), new CubePosition( 1,  0, -1),
      new CubePosition(-1,  1,  0), new CubePosition( 1, -1,  0),
      new CubePosition( 0,  1, -1), new CubePosition( 0, -1,  1)
    };

    public CubePosition(int x, int y, int z) {
      XPos = x;
      YPos = y;
      ZPos = z;
    }
    
    public CubePosition(Vector3 worldPosition) {
      var x = (float) (worldPosition.x * Mathf.Sqrt(3) / 3.0f - worldPosition.z / 3.0);
      XPos = (int) Mathf.Round(x/TileSize); 
      
      var z = (float) (worldPosition.z * 2.0f / 3.0f / TileSize);
      ZPos = (int) Mathf.Round(z);
      
      YPos = -XPos -ZPos;
    }

    public override Position[] GetNeighbours() {
      return Neighbours.Select(neighbour => new CubePosition(XPos + neighbour.XPos, YPos + neighbour.YPos, ZPos + neighbour.ZPos)).ToArray();
    }
    
    public override Vector3 GetWorldPosition() {
      var x = (float) (TileSize * Mathf.Sqrt(3) * (XPos + ZPos / 2.0));
      var y = (float) (TileSize * 3.0 / 2.0 * ZPos);
      return new Vector3(x, 0, y);
    }

    public bool Equals(CubePosition pos) {
      return (XPos == pos.XPos && YPos == pos.YPos && ZPos == pos.ZPos);
    }

    public override bool Equals(object o) {
      return Equals(o as CubePosition);
    }

    public override int GetHashCode() {
      unchecked {
        var hashCode = XPos;
        hashCode = (hashCode*397) ^ YPos;
        hashCode = (hashCode*397) ^ ZPos;
        return hashCode;
      }
    }

    public override string ToString() {
      return "Cube [ " + XPos + ", " + YPos + ", " + ZPos + "]";
    }
  }

  public class AxialPosition: CubePosition {

    public int Q {
      get {
        return XPos;
      }
      set {
        XPos = value;
      }
    }
    public int R {
      get {
        return ZPos;
      }
      set {
        ZPos = value;
      }
    }

    public AxialPosition(int q, int r) : base(q, -q-r, r) { }
    public AxialPosition(Vector3 worldPosition) : base(worldPosition) {}

    public override string ToString() {
      return "Axial [ " + Q + ", " + R + "]";
    }
  }
}