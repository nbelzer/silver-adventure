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
    grid.createGrid(10);
    
//    centerNode.runAction(SKAction.scaleBy(2, duration: 20));
  }

  override func touchesBegan(touches: Set<UITouch>, withEvent event: UIEvent?) {
    /* Called when a touch begins */
//    grid.createGrid(10)
  }
  
  override func touchesMoved(touches: Set<UITouch>, withEvent event: UIEvent?) {
    for touch in touches {
      let curLoc = touch.locationInView(self.view);
      let prevLoc = touch.previousLocationInView(self.view);
      
      let deltaX = curLoc.x - prevLoc.x;
      let deltaY = curLoc.y - prevLoc.y;
      
      if (abs(deltaX) > 3 || abs(deltaY) > 3) {
        grid.node.position.x += deltaX/2;
        grid.node.position.y -= deltaY/2;
      }
    }
  }

  override func update(currentTime: CFTimeInterval) {
    /* Called before each frame is rendered */
  }
}
