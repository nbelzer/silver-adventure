using System.Collections.Generic;
using System.Linq;

namespace Net.Mateybyrd.GameWorld.Grid
{
  /// <summary>
  /// A Position<T> class should implement the given methods and a way of storing the position.
  /// </summary>
  public abstract class Position<T> {

      /// <summary>
      /// Returns all the neighbours of this coordinate.
      /// </summary>
      public abstract T[] GetNeighbours();
    }

    public class GridPosition: Position<GridPosition> {

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

      public override GridPosition[] GetNeighbours() {
        return Neighbours.Select(neighbour => new GridPosition(XPos + neighbour.XPos, YPos + neighbour.YPos)).ToArray();
      }
    }

    public class CubePosition: Position<CubePosition> {

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

      public override CubePosition[] GetNeighbours() {
        return Neighbours.Select(neighbour => new CubePosition(XPos + neighbour.XPos, YPos + neighbour.YPos, ZPos + neighbour.ZPos)).ToArray();
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
    }
}