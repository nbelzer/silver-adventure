//
//  Pathfinder.swift
//  silver-adventure
//
//  Created by Nick Belzer on 25/02/16.
//  Copyright © 2016 MateyByrd.Net.
//

import Foundation
import GameplayKit

class Pathfinder {
  
  let nodeGraph: GKGraph;
  
  init() {
    nodeGraph = GKGraph();
  }
  
  func createNodeGraph(hexGrid: HexGrid) {
    
    var nodes: [GKGraphNode] = [];
    
    for (axial, tile) in hexGrid.grid {
      if (tile.tileType.walkable) {
        nodes.append(tile);
        
        for neighbour in getNeighbours(axial) {
          if let hex = hexGrid.grid[neighbour] {
            if hex.tileType.walkable {
              tile.addConnectionsToNodes([hex], bidirectional: true);
            }
          }
        }
      }
    }
    
    nodeGraph.addNodes(nodes);
  }
}