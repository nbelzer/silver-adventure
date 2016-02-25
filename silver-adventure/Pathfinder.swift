//
//  Pathfinder.swift
//  silver-adventure
//
//  Created by Nick Belzer on 25/02/16.
//  Copyright © 2016 MateyByrd.Net.
//

import Foundation
import GameplayKit
import UIKit;

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
  
  // TEMPORARY
  var highlightedNodes: [GKGraphNode] = [];
  func showPath(fromNode: GKGraphNode2D, toNode: GKGraphNode2D) {
    for node in highlightedNodes {
      if let tile = node as? Tile {
        tile.highlightSprite.color = tile.originalColor;
      }
    }
    
    let nodes = nodeGraph.findPathFromNode(fromNode, toNode: toNode)
    for node in nodes {
      if let tile = node as? Tile {
        tile.highlightSprite.color = UIColor.orangeColor();
      }
    }
    highlightedNodes = nodes;
  }
}