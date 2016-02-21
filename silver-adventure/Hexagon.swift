//
// Created by Nick Belzer on 02/02/16.
// Copyright (c) 2016 Nick Belzer. All rights reserved.
//

import Foundation
import SpriteKit;
import GameplayKit;

protocol Hexagon {
  var coordinate: Axialcoordinate { get set }
}

class Tile: Hexagon {
  
  let sprite: SKSpriteNode;
  var coordinate: Axialcoordinate;
  
  var height: Float;
  
  init(atCoordinate: Axialcoordinate, height: Float) {
    self.coordinate = atCoordinate;
    sprite = SKSpriteNode(imageNamed: "Hexagon");
    sprite.position = coordinate.toWorld();
    sprite.xScale = 0.2;
    sprite.yScale = sprite.xScale;
    sprite.colorBlendFactor = 1.0;
    sprite.color = UIColor(red: CGFloat(height), green: CGFloat(height), blue: CGFloat(height), alpha: 1)
    
    self.height = height;
  }
}