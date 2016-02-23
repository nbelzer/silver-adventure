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
  
  func getColor() -> UIColor {
    switch(self) {
    case .Land:
      return UIColor(hue: 0.32, saturation: 0.5, brightness: 0.6, alpha: 1.0);
    case .Water:
      return UIColor(hue: 0.55, saturation: 0.53  , brightness: 0.62, alpha: 1.0)
    case .Mountain:
      return UIColor(hue: 0, saturation: 0.0, brightness: 0.5, alpha: 1.0)
    }
  }
  
  var centerFactor: Float {
    switch(self) {
    case .Land:
      return 0.8;
    case .Water:
      return 0.0;
    case .Mountain:
      return 2.5;
    }
  }
  
  var spawnFactor: Float {
    switch(self) {
    case .Land:
      return 0.8;
    case .Water:
      return 1.0;
    case .Mountain:
      return 0.6;
    }
  }
}

class Tile: Hexagon {
  
  let sprite: SKSpriteNode;
  var coordinate: Axialcoordinate;
  let tileType: TileType;
  
  init(atCoordinate: Axialcoordinate, tileType: TileType) {
    self.coordinate = atCoordinate;
    self.tileType = tileType;
    sprite = SKSpriteNode(imageNamed: "Hexagon");
    sprite.position = coordinate.toWorld();
    sprite.xScale = 0.2;
    sprite.yScale = sprite.xScale;
    sprite.colorBlendFactor = 1.0;
    sprite.color = tileType.getColor();
  }
}