using UnityEngine;

namespace Phil.Core {

[System.Serializable]
public struct BezierCurve2D {
    public Vector2 p0;
    public Vector2 p1;
    public Vector2 p2;
    public Vector2 p3;

    public BezierCurve2D(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3){
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }

    public Vector2 GetPosition(float t){
        Vector2 p01 = Vector2.Lerp(p0, p1, t);
        Vector2 p12 = Vector2.Lerp(p1, p2, t);
        Vector2 p23 = Vector2.Lerp(p2, p3, t);
        Vector2 p012 = Vector2.Lerp(p01, p12, t);
        Vector2 p123 = Vector2.Lerp(p12, p23, t);
        Vector2 final = Vector2.Lerp(p012, p123, t);
        return final;
    }

    public void SetPosition(Vector2 value, int i){
        switch(i){
            case 0: p0 = value; break;
            case 1: p1 = value; break;
            case 2: p2 = value; break;
            case 3: p3 = value; break;
        }
    }

    public Vector2 GetVelocity(float t){
        float oneMinusT = 1f - t;
        float oneMinusT2 = oneMinusT*oneMinusT;
        float t2 = t*t;
        Vector2 velocity = -3f*p0*oneMinusT2 + 3f*p1*(oneMinusT2 - 2*t*oneMinusT) + 3f*p2*(2f*t*oneMinusT - t2) + 3f*p3*t2;
        return velocity;
    }

    public Vector2 GetDirection(float t){
        return GetVelocity(t).normalized;
    }
}

[System.Serializable]
public struct BezierCurve3D {
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;

    public BezierCurve3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3){
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }
    
    public static BezierCurve3D FromPointsAndTangents(Vector3 start, Vector3 posStartTangent,
        Vector3 end, Vector3 posEndTangent)
    {
        return new BezierCurve3D(start, start+posStartTangent, end-posEndTangent, end);
    }

    public Vector3 GetPosition(float t){
        Vector3 p01 = Vector3.Lerp(p0, p1, t);
        Vector3 p12 = Vector3.Lerp(p1, p2, t);
        Vector3 p23 = Vector3.Lerp(p2, p3, t);
        Vector3 p012 = Vector3.Lerp(p01, p12, t);
        Vector3 p123 = Vector3.Lerp(p12, p23, t);
        Vector3 final = Vector3.Lerp(p012, p123, t);
        return final;
    }

    public Vector3 GetVelocity(float t){
        float oneMinusT = 1f - t;
        float oneMinusT_2 = oneMinusT*oneMinusT;
        float t2 = t*t;
        Vector3 velocity = -3f*p0*oneMinusT_2 + 3f*p1*(oneMinusT_2 - 2*t*oneMinusT) + 3f*p2*(2f*t*oneMinusT - t2) + 3f*p3*t2;
        return velocity;
    }

    public Vector3 GetAcceleration(float t){
        Vector3 alpha = -p0 + 3f*p1 - 3*p2 + p3;
        Vector3 beta = 3f*p0 - 6f*p1 + 3*p2;
        Vector3 gamma = -3*p0 + 3*p1;
        return (6f*alpha*t) + (2f*beta);
    }

    public Vector3 GetDirection(float t){
        return GetVelocity(t).normalized;
    }

    public Vector3 GetStartTangent(){ return p1 - p0; }
    public Vector3 GetEndTangent(){ return p3 - p2; }

    // return false if the maxRadius is exceeded or the denominator is zero;
    public bool TryCalcCurvature(float t, float maxRadius, out float radiusOfCurvature){
        Vector3 rPrime = GetVelocity(t);
        Vector3 rDoublePrime = GetAcceleration(t);
        float numerator = Vector3.Cross(rPrime, rDoublePrime).magnitude;
        float speed = rPrime.magnitude;
        float denominator = speed*speed*speed;
        if(denominator == 0f){
            radiusOfCurvature = 0f;
            return false;
        }
        radiusOfCurvature = numerator / denominator;
        if(radiusOfCurvature > maxRadius){
            return false;
        }
        return true;
    }

    public void ScaleAbout(Vector3 scaleCenter, float scaleFactor){
        p0 = scaleFactor * (p0-scaleCenter) + scaleCenter;
        p1 = scaleFactor * (p1-scaleCenter) + scaleCenter;
        p2 = scaleFactor * (p2-scaleCenter) + scaleCenter;
        p3 = scaleFactor * (p3-scaleCenter) + scaleCenter;
    }

    public float EstimateLength(int iterationCount){
        float sum = 0f;
        float deltaT = 1f / iterationCount;
        for(int i = 0; i < iterationCount; i++){
            float a_t = i*deltaT;
            float b_t = (i+1)*deltaT;
            Vector3 a = GetPosition(a_t);
            Vector3 b = GetPosition(b_t);
            sum += Vector3.Distance(a,b);
        }
        return sum;
    }

    public void Draw(int lineSegCount, Color color){
        float deltaT = 1f / lineSegCount;
        for(int i = 0; i < lineSegCount; i++){
            float a_t = i*deltaT;
            float b_t = (i+1)*deltaT;
            Debug.DrawLine( GetPosition(a_t), GetPosition(b_t), color );
        }
    }

    public static BezierCurve3D Lerp(BezierCurve3D a, BezierCurve3D b, float t){
        return new BezierCurve3D(
            Vector3.Lerp(a.p0, b.p0, t),
            Vector3.Lerp(a.p1, b.p1, t),
            Vector3.Lerp(a.p2, b.p2, t),
            Vector3.Lerp(a.p3, b.p3, t)
        );
    }
}

}