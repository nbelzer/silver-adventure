//
// Created by Nick Belzer on 02/02/16.
// Copyright (c) 2016 Nick Belzer. 
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
//  case Sand
  case Mountain
  
  func getColor() -> (hue: CGFloat, saturation: CGFloat, brightness: CGFloat, alpha: CGFloat) {
    switch(self) {
    case .Land:
      return (hue: 0.32, saturation: 0.5, brightness: 0.6, alpha: 1.0);
    case .Water:
      return (hue: 0.55, saturation: 0.53, brightness: 0.62, alpha: 1.0)
//    case .Sand:
//      return (hue: 0.14, saturation: 0.32, brightness: 0.62, alpha: 1.0)
    case .Mountain:
      return (hue: 0, saturation: 0.0, brightness: 0.5, alpha: 1.0)
    }
  }
  
  var centerFactor: Float {
    switch(self) {
    case .Land:
      return 0.8;
    case .Water:
      return 0.0;
//    case .Sand:
//      return 0.0;
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
//    case .Sand:
//      return 0.0;
    case .Mountain:
      return 0.6;
    }
  }
  
  var walkable: Bool {
    switch (self) {
    case .Land:
      return true;
    default:
      return false;
    }
  }
}

class Tile: GKGraphNode2D, Hexagon {
  
  let sprite: SKSpriteNode;
  var coordinate: Axialcoordinate;
  let tileType: TileType;
  
  init(atCoordinate: Axialcoordinate, tileType: TileType) {
    let worldPosition = atCoordinate.toWorld();
    
    self.coordinate = atCoordinate;
    self.tileType = tileType;
    sprite = SKSpriteNode(imageNamed: "Hexagon");
    sprite.position = worldPosition;
    sprite.xScale = 0.2;
    sprite.yScale = sprite.xScale;
    sprite.colorBlendFactor = 1.0;
    
    let color = tileType.getColor();
    sprite.color = UIColor(hue: color.hue,
                           saturation: color.saturation,
                           brightness: color.brightness + CGFloat(arc4random_uniform(10))/100,
                           alpha: color.alpha)
    
    super.init(point: vector_float2(Float(worldPosition.x), Float(worldPosition.y)));
  }
}