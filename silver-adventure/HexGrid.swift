//
// Created by Nick Belzer on 02/02/16.
// Copyright (c) 2016 Nick Belzer. All rights reserved.
//

import Foundation
import SpriteKit
import GameKit

class HexGrid {

  let generator: GridGenerator = GridGenerator();
  let pathfinder: Pathfinder = Pathfinder();
  var grid = [Axialcoordinate: Tile]()
  
  let node: SKNode;
  
  init(toNode: SKNode) {
    node = toNode;
  }
  
  func createGrid(size: Int) {
    for (_, hex) in grid {
      hex.sprite.removeFromParent();
    }
    grid = [:];

    generator.createGrid(size, createHexagon: createHexagon);

    addToNode(node)
    
    pathfinder.createNodeGraph(self);
    
    let nodes = pathfinder.nodeGraph.findPathFromNode(grid[Axialcoordinate(q: 3,r: 3)]!, toNode: grid[Axialcoordinate(q: -3,r: -3)]!)
    for node in nodes {
      if let tile = node as? Tile {
        tile.sprite.color = UIColor.orangeColor();
      }
    }
  }
  
  func addToNode(node: SKNode) {
    for (_, hex) in grid {
      node.addChild(hex.sprite)
    }
  }
  
  func createHexagon(atPosition: Axialcoordinate, typeof: TileType) {
    grid[atPosition] = Tile(atCoordinate: atPosition, tileType: typeof);
  }
}
