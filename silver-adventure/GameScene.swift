//
//  GameScene.swift
//  silver-adventure
//
//  Created by Nick Belzer on 21/02/16.
//  Copyright (c) 2016 MateyByrd.Net.
//

import SpriteKit

class GameScene: SKScene {
  
  var grid: HexGrid!;
  
  override func didMoveToView(view: SKView) {
    /* Setup your scene here */
    let centerNode = SKNode()
    centerNode.position = CGPoint(x: CGRectGetMidX(self.frame), y: CGRectGetMidY(self.frame))

    self.addChild(centerNode)
    
    grid = HexGrid(toNode: centerNode)
    grid.createGrid(8);
  }

  override func touchesBegan(touches: Set<UITouch>, withEvent event: UIEvent?) {
    /* Called when a touch begins */
    grid.createGrid(8)
  }

  override func update(currentTime: CFTimeInterval) {
    /* Called before each frame is rendered */
  }
}
