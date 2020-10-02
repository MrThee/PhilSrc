using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class BezierChain3D {
    public readonly List<BezierCurve3D> curves;

    public BezierChain3D(int curveCountCapacity){
        this.curves = new List<BezierCurve3D>(curveCountCapacity);
    }

    // Mutators

    public void Copy(BezierChain3D rhs){
        this.curves.Clear();
        foreach(var c in rhs.curves){
            this.curves.Add(c);
        }
    }

    public void Clear(){
        this.curves.Clear();
    }

    public void AppendContinuousExtension(Vector3 endPoint, Vector3 endPositiveTangentVector){
        int cCount = curves.Count;
        if(curves.Count == 0){
            Debug.LogErrorFormat("ERROR: Cannot append an extension to an empty chain. You need at least one curve before you can use this function");
            return;
        }
        var curEndSegment = curves[cCount-1];
        Vector3 newStartTangent = curEndSegment.p3 - curEndSegment.p2;
        
        Vector3 p0 = curEndSegment.p3;
        Vector3 p1 = p0 + newStartTangent;
        Vector2 p2 = endPoint - endPositiveTangentVector;
        Vector3 p3 = endPoint;
        curves.Add(new BezierCurve3D(p0, p1, p2, p3));
    }

    public void SetTangentVector(int fencepostIndex, Vector3 positiveTangentVector){
        if(fencepostIndex < 0 || fencepostIndex > curves.Count){
            Debug.LogErrorFormat("ERROR: fencepostIndex {0} must be in the inclusive range [0, {1}].", 
                fencepostIndex, curves.Count
            );
        }
        
        if(fencepostIndex < 1){
            BezierCurve3D startCurve = curves[0];
            startCurve.p1 = startCurve.p0 + positiveTangentVector;
            curves[0] = startCurve;
        } else if(1 <= fencepostIndex && fencepostIndex < curves.Count) {
            BezierCurve3D preCurve = curves[fencepostIndex-1];
            BezierCurve3D postCurve = curves[fencepostIndex];
            preCurve.p2 = preCurve.p3 - positiveTangentVector;
            postCurve.p1 = postCurve.p0 + positiveTangentVector;
            curves[fencepostIndex-1] = preCurve;
            curves[fencepostIndex] = postCurve;
        } else {
            BezierCurve3D endCurve = curves[fencepostIndex];
            endCurve.p2 = endCurve.p3 - positiveTangentVector;
            curves[fencepostIndex] = endCurve;
        }
    }

    // Accessors

    private int GetCurveIndex(float t){
        return Phil.Math.FloorToIndex(t, curves.Count);
    }

    private float GetInnerT(int curveIndex, float outer_t){
        float tPerCurve = 1f / curves.Count;
        float lower_t = tPerCurve * curveIndex;
        float into_t = outer_t - lower_t;
        return into_t / tPerCurve;
    }

    private (BezierCurve3D, float) GetCurveAndT(float t){
        int ci = GetCurveIndex(t);
        float i_t = GetInnerT(ci, t);
        return System.ValueTuple.Create(curves[ci], i_t);
    }

    public Vector3 GetPosition(float t){
        var (curve, i_t) = GetCurveAndT(t);
        return curve.GetPosition(i_t);
    }

    public Vector3 GetVelocity(float t){
        var (curve, i_t) = GetCurveAndT(t);
        return curve.GetVelocity(i_t);
    }

    public Vector3 GetAcceleration(float t){
        var (curve, i_t) = GetCurveAndT(t);
        return curve.GetAcceleration(i_t);
    }

    public Vector3 GetDirection(float t){
        var (curve, i_t) = GetCurveAndT(t);
        return curve.GetDirection(i_t);
    }

    // return false if the maxRadius is exceeded or the denominator is zero;
    public bool TryCalcCurvature(float t, float maxRadius, out float radiusOfCurvature){
        var (curve, i_t) = GetCurveAndT(t);
        return curve.TryCalcCurvature(i_t, maxRadius, out radiusOfCurvature);
    }

    public void ScaleAbout(Vector3 scaleCenter, float scaleFactor){
        int cCount = curves.Count;
        for(int i = 0; i < cCount; i++){
            var curve = curves[i];
            curve.ScaleAbout(scaleCenter, scaleFactor);
            curves[i] = curve;
        }
    }

    public static bool CanLerp(BezierChain3D a, BezierChain3D b){
        return (a.curves.Count == b.curves.Count);
    }

    // TODO: implement subdivide/upres

    public static void Lerp(BezierChain3D a, BezierChain3D b, float t, ref BezierChain3D dst){
        if(a.curves.Count != b.curves.Count){
            Debug.LogErrorFormat("Currently cannot lerp 2 chains of different curveCount. A curveCount: {0}, B curveCount: {1} ",
                a.curves.Count, b.curves.Count
            );
            return;
        }

        dst.curves.Clear();
        int cCount = a.curves.Count;
        for(int i = 0; i < cCount; i++){
            var aCurve = a.curves[i];
            var bCurve = a.curves[i];
            dst.curves.Add( BezierCurve3D.Lerp(aCurve, bCurve, t) );
        }
    }

    public float EstimateRoughLength(int iterationsOverWholeChain){
        float sum = 0f;
        float deltaT = 1f / iterationsOverWholeChain;
        for(int i = 0; i < iterationsOverWholeChain; i++){
            float a_t = i * deltaT;
            float b_t = (i+1) * deltaT;
            Vector3 a = GetPosition(a_t);
            Vector3 b = GetPosition(b_t);
            sum += Vector3.Distance(a,b);
        }
        return sum;
    }

    public float EstimatePreciseLength(int iterationsPerSubcurve){
        float sum = 0f;
        foreach(var curve in curves){
            sum += curve.EstimateLength(iterationsPerSubcurve);
        }
        return sum;
    }

    public void Draw(int lineSegPerCurve, Color color){
        foreach(var curve in curves){
            curve.Draw(lineSegPerCurve, color);
        }
    }
}

}