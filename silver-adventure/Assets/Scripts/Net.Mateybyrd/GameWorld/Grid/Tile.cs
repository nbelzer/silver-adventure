using System;
using System.Collections.Generic;
using UnityEngine;

namespace Net.Mateybyrd.GameWorld.Grid {
  
  public class Tile<Pos> {
    
    public Vector3 worldPosition;
    public Pos gridPosition; 
    
    public Tile() {
      
    }
  }
  
  public abstract class Position<Type> {
    public abstract Type[] GetNeighbours();
  }
  
  public class GridPosition: Position<GridPosition> {
    
    // Store position
    public int xPos, yPos;
    
    // Neighbouring tile offsets
    private static GridPosition[] _neighbours = { 
      new GridPosition(-1,  0), new GridPosition( 1,  0), 
      new GridPosition( 0,  1), new GridPosition( 0, -1)
    };
    
    public GridPosition(int x, int y) {
      xPos = x;
      yPos = y;
    }
    
    public override GridPosition[] GetNeighbours() {
      var neighbours = new List<GridPosition>();
      foreach (var neighbour in _neighbours) {
        neighbours.Add(new GridPosition(xPos + neighbour.xPos, yPos + neighbour.yPos));
      }
      return neighbours.ToArray();
    }
  }
  
  public class CubePosition: Position<CubePosition> {
   
    // Store position
    public int xPos, yPos, zPos;
    
    private static CubePosition[] _neighbours = {
      new CubePosition(-1,  0,  1), new CubePosition( 1,  0, -1), 
      new CubePosition(-1,  1,  0), new CubePosition( 1, -1,  0),
      new CubePosition( 0,  1, -1), new CubePosition( 0, -1,  1)
    };
    
    public CubePosition(int x, int y, int z) {
      xPos = x;
      yPos = y;
      zPos = z;
    }
    
    public override CubePosition[] GetNeighbours() {
      var neighbours = new List<CubePosition>();
      foreach (var neighbour in _neighbours) {
        neighbours.Add(new CubePosition(xPos + neighbour.xPos, yPos + neighbour.yPos, zPos + neighbour.zPos));
      }
      return neighbours.ToArray();
    }
  }
  
  public class AxialPosition: CubePosition {
    
    public int q {
      get {
        return this.xPos;
      }
      set {
        this.xPos = value;
      }
    }
    public int r {
      get {
        return this.zPos;
      }
      set {
        this.zPos = value;
      }
    }
    
    public AxialPosition(int q, int r) : base(q, -q-r, r) { }
  }
}