using System;
using System.Collections.Generic;
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
    
    public abstract Position[] GetInRange(int range);
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
    
    public override Position[] GetInRange(int range) {
      Debug.LogError("Method not implemented, you beter be fixing this sometime");
      return null;
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
    public float XPos, YPos, ZPos;

    private static readonly CubePosition[] Neighbours = {
      new CubePosition(-1,  0,  1), new CubePosition( 1,  0, -1),
      new CubePosition(-1,  1,  0), new CubePosition( 1, -1,  0),
      new CubePosition( 0,  1, -1), new CubePosition( 0, -1,  1)
    };

    public CubePosition(float x, float y, float z) {
      XPos = x;
      YPos = y;
      ZPos = z;
    }
    
    public static CubePosition CubeFromWorld(Vector3 worldPosition) {
      var x = (float) (worldPosition.x * Mathf.Sqrt(3) / 3.0f - worldPosition.z / 3.0) / TileSize;
      var z = (float) (worldPosition.z * 2.0f / 3.0f / TileSize);
      var y = -x -z;
      
      return RoundCube(new CubePosition(x, y, z));
    }

    public override Position[] GetNeighbours() {
      return Neighbours.Select(neighbour => new CubePosition(XPos + neighbour.XPos, YPos + neighbour.YPos, ZPos + neighbour.ZPos)).ToArray();
    }
    
    public override Vector3 GetWorldPosition() {
      var x = (float) (TileSize * Mathf.Sqrt(3) * (XPos + ZPos / 2.0));
      var y = (float) (TileSize * 3.0 / 2.0 * ZPos);
      return new Vector3(x, 0, y);
    }
    
    public override Position[] GetInRange(int range) {
      var results = new List<CubePosition>();
      for (var dx = -range; dx <= range; dx++) {
        for (var dy = Mathf.Max(-range, -dx-range); dy <= Mathf.Min(range, -dx+range); dy++) {
          var dz = -dx-dy;
          results.Add(new CubePosition(XPos + dx, YPos + dy, ZPos + dz));
        }
      }
      return results.ToArray();
    }
    
    public static CubePosition RoundCube(CubePosition c) {
      var rx = Mathf.Round(c.XPos);
      var ry = Mathf.Round(c.YPos);
      var rz = Mathf.Round(c.ZPos);
      
      var x_diff = Mathf.Abs(rx - c.XPos);
      var y_diff = Mathf.Abs(ry - c.YPos);
      var z_diff = Mathf.Abs(rz - c.ZPos);
      
      if (x_diff > y_diff && x_diff > z_diff) {
        rx = -ry-rz;
      } else if (y_diff > z_diff) {
        ry = -rx-rz;
      } else {
        rz = -rx-ry;
      }
      return new CubePosition(rx, ry, rz);
    }
    
    public AxialPosition ToAxial() {
      return new AxialPosition(XPos, ZPos);
    }

    public bool Equals(CubePosition pos) {
      return (XPos == pos.XPos && YPos == pos.YPos && ZPos == pos.ZPos);
    }

    public override bool Equals(object o) {
      return Equals(o as CubePosition);
    }

    public override int GetHashCode() {
      unchecked {
        var hashCode = (int)XPos;
        hashCode = (hashCode*397) ^ ((int)YPos);
        hashCode = (hashCode*397) ^ ((int)ZPos);
        return hashCode;
      }
    }

    public override string ToString() {
      return "Cube [ " + XPos + ", " + YPos + ", " + ZPos + "]";
    }
  }

  public class AxialPosition: CubePosition {

    public float Q {
      get {
        return XPos;
      }
      set {
        XPos = value;
      }
    }
    public float R {
      get {
        return ZPos;
      }
      set {
        ZPos = value;
      }
    }

    public AxialPosition(float q, float r) : base(q, -q-r, r) { }
    public static AxialPosition AxialFromWorld(Vector3 worldPosition) {
      return CubeFromWorld(worldPosition).ToAxial();
    }

    public override string ToString() {
      return "Axial [ " + Q + ", " + R + "]";
    }
  }
}