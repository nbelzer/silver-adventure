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

enum TileType {
  case Water
  case Land
  case Mountain
}

class Tile: Hexagon {
  
  let sprite: SKSpriteNode;
  var coordinate: Axialcoordinate;
  let tileType: TileType;
  
  var height: Float;
  
  init(atCoordinate: Axialcoordinate, height: Float) {
    self.coordinate = atCoordinate;
    sprite = SKSpriteNode(imageNamed: "Hexagon");
    sprite.position = coordinate.toWorld();
    sprite.xScale = 0.2;
    sprite.yScale = sprite.xScale;
    sprite.colorBlendFactor = 1.0;
//    sprite.color = UIColor(red: CGFloat(height), green: 0.4, blue: 0.5, alpha: 1)
    
    self.height = height;

    if (height <= 0.4) { tileType = TileType.Water }
    else if (height <= 0.7) { tileType = TileType.Land }
    else { tileType = TileType.Mountain }
    
    switch (tileType) {
    case TileType.Water:
      sprite.color = UIColor(hue: 205/360.0, saturation: 0.8, brightness: CGFloat(height/0.8) + 0.35, alpha: 1.0)
      break
    case TileType.Land:
      sprite.color = UIColor(hue: 85/360.0, saturation: 1.0, brightness: CGFloat((height-0.4)/0.7) + 0.5, alpha: 1.0)
      break
    case TileType.Mountain:
      sprite.color = UIColor(hue: 0/360.0, saturation: 0, brightness: CGFloat((height-0.7)/0.9) + 0.2, alpha: 1.0)
      break
    }
  }
}