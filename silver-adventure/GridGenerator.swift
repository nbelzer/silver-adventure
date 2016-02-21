//
// Created by Nick Belzer on 21/02/16.
// Copyright (c) 2016 MateyByrd.Net.
//

import Foundation

class GridGenerator {

  func createGrid(withSize: Int, createHexagon:(atPosition: Axialcoordinate)->()) {
    
    for x in -withSize...withSize {
      for y in -withSize...withSize {
        let z = -x-y;

        if abs(z) <= withSize {
          let axial = Axialcoordinate(q: x, r: z);
          createHexagon(atPosition: axial);
        }
      }
    }
  }
}
