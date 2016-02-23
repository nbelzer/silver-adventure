//
// Created by Nick Belzer on 02/02/16.
// Copyright (c) 2016 Nick Belzer. All rights reserved.
//

import Foundation
import SpriteKit
import GameKit

class HexGrid {

  let generator: GridGenerator = GridGenerator();
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
