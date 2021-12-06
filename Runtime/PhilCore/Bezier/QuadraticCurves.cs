using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {
    
public struct QuadraticCurve3D
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;

    public QuadraticCurve3D(Vector3 p0, Vector3 p1, Vector3 p2){
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
    }

    public Vector3 GetPosition(float t){
        return Vector3.Lerp( Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t );
    }

    public Vector3 GetVelocity(float t){
        return 2f*(1f-t)*(p1-p0) + 2f*t*(p2-p1);
        // 2(1-t)(b-a) + 2t(c-b)
        // 2b - 2a -2bt + 2at + 2ct - 2bt
        // (-4b + 2a + 2c)t + 2(b-a)
        // 
    }

    public Vector3 GetDirection(float t){
        return GetVelocity(t).normalized;
    }

    // public float GetClosestParametricValue(Vector3 point){
        // B(t) = b + (1-t)^2(a-b) + t^2(c-b)
        //      = b + (1-2t+t^2)(a-b) + t^2(c-b)
        //      = b + (a - b - 2at + 2bt + at^2 - bt^2) + ct^2 - bt^2
        //      = a - 2at + 2bt + at^2 - 2bt^2 + ct^2
        //      = (a-2b+c)t^2 + 2(b-a)t + a
        // B'(t)= 2(a-2b+c)t + 2(b-a)
        // R^2 = (Bx(t) - px)^2 + ...y + ...z
        // R^2 = (Bx(t))^2 - 2Bx(t)px + px^2 + ...
        // d(R^2)/dt = 2Bx'(t)Bx(t) - 2Bx'(t)px + ...
        // 0 = 2Bx'(t)Bx(t) - 2Bx'(t)px + ...
        
        // TermX = 2Bx'(t)Bx(t) - 2Bx'(t)px
        // TermX = 2[2(ax-2bx+cx)t + 2(bx-ax)][(a-2b+c)t^2 + 2(b-a)t + a] - 2[2(ax-2bx+cx)t + 2(bx-ax)]px
        // A = (ax-2bx+cx)
        // B = (bx-ax)
        // TermX = 2[2At + 2B][At^2 + 2Bt + a] - 2[2At + 2B]px
        //       = 2(2A^2t^3 + 4ABt^2 + 2Aat + 2ABt^2 + 4B^2t + 2Ba) - 4Apxt + 4Bpx
        //       = 2(2A^2t^3 + 6ABt^2 + (2Aa + 4B^2)t + 2Ba) - 4Apxt + 4Bpx
        //       = 4A^2t^3 + 12ABt^2 + (4Aa + 8B^2)t + 4Ba - 4Apxt + 4Bpx
        // TermX = (4A^2)t^3 + (12AB)t^2 + (4Aa + 8B^2 - 4Apx)t + (4Ba + 4Bpx)
    // }
}

}