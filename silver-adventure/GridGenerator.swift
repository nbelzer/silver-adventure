//
// Created by Nick Belzer on 21/02/16.
// Copyright (c) 2016 MateyByrd.Net.
//

import Foundation
import GameplayKit

class GridGenerator {
  
  private var tempGrid: [Axialcoordinate: TileType] = [:];
  private var tempSize: Int = 1;
  let random: GKRandomDistribution;
  
  init() {
    random = GKGaussianDistribution(randomSource: GKRandomSource(), mean: 9, deviation: 3)
  }

  func createGrid(withSize: Int, createHexagon:(atPosition: Axialcoordinate, typeof: TileType)->()) {
    tempGrid = [:];
    tempSize = withSize;
    
    addLayer(withSize + 2, layerType: TileType.Water)
    addLayer(withSize, layerType: TileType.Land)
    addLayer(withSize, layerType: TileType.Mountain)
    
    buildGrid(createHexagon);
  }
  
  func addLayer(size: Int, layerType: TileType) {
    for x in -size...size {
      for y in -size...size {
        let z = -x-y;
        
        if abs(z) <= size {
          let axial = Axialcoordinate(q: x, r: z);
          addHexToGrid(axial, layerType: layerType);
        }
      }
    }
  }
  
  func addHexToGrid(atPosition: Axialcoordinate, layerType: TileType) {
    let x: Float = abs(Float(atPosition.q^2) / Float(3 * tempSize^2));
    let y: Float = abs(Float(atPosition.r^2) / Float(3 * tempSize^2));
    let z: Float = abs(Float((-atPosition.q-atPosition.r)^2) / Float(3 * tempSize^2));
    let chance: Float = (x + y + z) * layerType.centerFactor;
    
    if (random.nextUniform() >= chance) {
      if (random.nextUniform() <= layerType.spawnFactor) {
        tempGrid[atPosition] = layerType
      }
    }
  }
  
  func buildGrid(createHexagon:(atPosition: Axialcoordinate, typeof: TileType)->()) {
    
    for (coordinate, type) in tempGrid {
      createHexagon(atPosition: coordinate, typeof: type);
    }
  }
}
