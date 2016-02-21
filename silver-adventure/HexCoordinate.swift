//
// Created by Nick Belzer on 02/02/16.
// Copyright (c) 2016 Nick Belzer. All rights reserved.
//
// Contains two ways of storing a coordinate system in a hexagon grid.
//

import Foundation

/// Cubecoordinate
///
/// A coordinate system used for a hexagon grid. It contains three axis (x, y, z) which makes sense when looking at a hexagon, a hexagon contains six sides and thus when looking at a grid you can move in three directions.
///
/// The cubecoordinate is mostly used for calculation, for example when calulating a path from one place to the other.
struct Cubecoordinate: Equatable, Hashable {

  var x, y, z : Int

  init(x: Int, y: Int, z: Int) {
    self.x = x
    self.y = y
    self.z = z
  }

  func toAxial() -> Axialcoordinate {
    return Axialcoordinate(q: x, r: z)
  }

  var hashValue: Int {
    return x.hashValue &+ y.hashValue &+ z.hashValue
  }
}

func ==(lhs: Cubecoordinate, rhs: Cubecoordinate) -> Bool {
  return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z
}

/* Axialcoordinate

  A coordinate system based on a simple rule to convert cubecoordinates. To make things easier this system only contains two axis while still containing the same information as a cubecoordinate. To make two axis out of three we have to apply a simple rule where (x + y + z = 0). When using this rule we can convert the cubecoordinate to be like this (x + 0 + z = -y), because the 0 will allways stay the same we now only have to store the x and z values and when needed we can calculate the y value because we know the other three values.

  The axialcoordinate is mostly used for storing a grid of hexagons because it simply contains less variables.
*/

struct Axialcoordinate: Equatable, Hashable {

  var q, r : Int

  init(q: Int, r: Int) {
    self.q = q
    self.r = r
  }

  func toCube() -> Cubecoordinate {
    return Cubecoordinate(x: q, y: -q-r, z: r)
  }

  var hashValue: Int {
    return q.hashValue &+ r.hashValue
  }
}

func ==(lhs: Axialcoordinate, rhs: Axialcoordinate) -> Bool {
  return lhs.q == rhs.q && lhs.r == rhs.r
}