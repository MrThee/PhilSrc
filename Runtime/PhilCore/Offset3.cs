using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

// This is the safety dual of the Point3 class
public struct Offset3 {

    public Vector3 value;

    public Offset3(float x, float y, float z){
        this.value = new Vector3(x,y,z);
    }

    public Offset3(Vector3 vec){
        this.value = vec;
    }

    public static implicit operator Offset3(Vector3 vec){
        return new Offset3(vec);
    }

    public static Offset3 operator+(Offset3 a, Offset3 b){
        return new Offset3(a.value+b.value);
    }

    // Safety strat B
    public static Offset3 operator-(Offset3 a, Offset3 b){
        return new Offset3(a.value - b.value);
    }

    public static Offset3 operator*(Offset3 a, Offset3 b){
        return new Offset3(
            a.value.x * b.value.x, 
            a.value.y * b.value.y, 
            a.value.z * b.value.z
        );
    }

    public static Offset3 operator*(Offset3 a, float b){
        return new Offset3(a.value*b);
    }
    public static Offset3 operator*(float a, Offset3 b){
        return new Offset3(a*b.value);
    }

    public static Offset3 operator/(Offset3 a, float b){
        return new Offset3(a.value/b);
    }

}

}