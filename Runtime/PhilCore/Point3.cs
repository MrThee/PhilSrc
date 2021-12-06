using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

// This is effectively a safety wrapper when doing lots of Vector math
// Prevents you from adding/multiplying/dividing points to points
// Subtracting A point from another point will yield an Offset3
public struct Point3 {
    public Vector3 value;

    public Point3(float x, float y, float z){
        this.value = new Vector3(x,y,z);
    }

    public Point3(Vector3 v) : this(v.x, v.y, v.z) {}

    public static implicit operator Point3(Vector3 vec){
        return new Point3(vec);
    }

    // Safety strat A
    public static Point3 operator+(Point3 point, Offset3 offset){
        return new Point3(point.value + offset.value);
    }

    public static Point3 operator+(Offset3 offset, Point3 point){
        return new Point3(point.value + offset.value);
    }

    public static Point3 operator-(Point3 point, Offset3 offset){
        return new Point3(point.value - offset.value);
    }

    public static Offset3 operator-(Point3 a, Point3 b){
        return new Offset3(a.value - b.value);
    }

}

}