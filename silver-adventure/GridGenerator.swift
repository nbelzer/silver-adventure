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
  
  init(seed: NSData) {
    random = GKGaussianDistribution(randomSource: GKARC4RandomSource(seed: seed), mean: 9, deviation: 3)
  }

  func createGrid(withSize: Int, createHexagon:(atPosition: Axialcoordinate, typeof: TileType)->()) {
    tempGrid = [:];
    tempSize = withSize;
    
    addLayer(withSize + 2, layerType: TileType.Water)
    addLayer(withSize, layerType: TileType.Land)
    addLayer(withSize, layerType: TileType.Mountain)
    
    replaceGroups(TileType.Water, replaceWith: TileType.Land, replaceWhenSmaller: 3);
    replaceGroups(TileType.Mountain, replaceWith: TileType.Land, replaceWhenSmaller: 2)
    
//    replaceTilesNeighbouring(TileType.Water, with: TileType.Sand, amountOfNeighbours: 3);
//    replaceGroups(TileType.Sand, replaceWith: TileType.Land, replaceWhenSmaller: 2)
    
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
  
  func replaceGroups(ofType: TileType, replaceWith: TileType, replaceWhenSmaller: Int) {
    let groups = groupLayer(ofType);
    
    for group in groups {
      if (group.count < replaceWhenSmaller) {
        for axial in group {
          tempGrid[axial] = replaceWith;
        }
      }
    }
  }
  
  func groupLayer(ofType: TileType) -> [[Axialcoordinate]] {
    var groups: [[Axialcoordinate]] = [];
    var searched: [Axialcoordinate] = [];
    
    for (axial, type) in tempGrid {
      if type == ofType {
        if (!searched.contains(axial)) {
          let area = checkArea(axial, ofType: ofType);
          searched.appendContentsOf(area);
          groups.append(area);
        }
      }
    }
    
    return groups;
  }
  
  func checkArea(atPosition: Axialcoordinate, ofType: TileType) -> [Axialcoordinate] {
    var candidates: [Axialcoordinate] = [atPosition];
    var area: [Axialcoordinate] = [];
    
    while candidates.count > 0 {
      if let coordinate = candidates.popLast() {
        if !area.contains(coordinate) {
          if let type = tempGrid[coordinate] {
            if (type == ofType) {
              area.append(coordinate);
              candidates.appendContentsOf(getNeighbours(coordinate));
            }
          }
        }
      }
    }
    
    return area;
  }
  
  func replaceTilesNeighbouring(tileType: TileType, with: TileType, amountOfNeighbours: Int) {
    
    for (axial, type) in tempGrid {
      if (type != tileType) {
        var amount = 0;
        for neighbour in getNeighbours(axial) {
          if let neighbouringHex = tempGrid[neighbour] {
            if (neighbouringHex == tileType) {
              amount += 1;
            }
          }
        }
        
        if (amount >= amountOfNeighbours) {
          tempGrid[axial] = with;
        }
      }
    }
  }
  
  func buildGrid(createHexagon:(atPosition: Axialcoordinate, typeof: TileType)->()) {
    
    for (coordinate, type) in tempGrid {
      createHexagon(atPosition: coordinate, typeof: type);
    }
  }
}
