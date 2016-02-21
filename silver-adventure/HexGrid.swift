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
  
  let random: GKRandomDistribution;
  let node: SKNode;
  
  init(toNode: SKNode) {
    random = GKRandomDistribution(lowestValue: 0, highestValue: 100);
    node = toNode;
  }
  
  func createGrid(size: Int) {
    for (_, hex) in grid {
      hex.sprite.removeFromParent();
    }

    generator.createGrid(size, createHexagon: createHexagon);

    addToNode(node)
  }
  
  func addToNode(node: SKNode) {
    for (_, hex) in grid {
      node.addChild(hex.sprite)
    }
  }
  
  func createHexagon(atPosition: Axialcoordinate) {
    grid[atPosition] = Tile(atCoordinate: atPosition, height: random.nextUniform());
  }
}
