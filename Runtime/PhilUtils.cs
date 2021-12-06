using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.CompilerServices;

namespace Phil {

public static class Utils {

    // return true if the current velocity overshoots the target position and got clamped
    public static bool ClampVelocityToTarget(ref float velocityToClamp, in float dt, in float startPos, in float targetPos){
        float targetDelta = targetPos - startPos;
        float currentEstimatedDelta = velocityToClamp * dt;
        if( Mathf.Abs(targetDelta) <= Mathf.Abs(currentEstimatedDelta) ){
            velocityToClamp = (dt == 0f) ? 0 : (targetDelta / dt);
            return true;
        }
        return false;
    }

    public static bool ClampVelocityToTarget(ref Vector2 velocityToClamp, in float dt, in Vector2 startPos, in Vector2 targetPos){
        Vector2 targetDelta = (targetPos - startPos);
        float targetDeltaMagnitude = targetDelta.magnitude;
        float currentEstimatedDeltaMagnitude = velocityToClamp.magnitude * dt;
        if( targetDeltaMagnitude <= currentEstimatedDeltaMagnitude ){
            velocityToClamp = (dt == 0f) ? Vector2.zero : (targetDelta / dt);
            return true;
        }
        return false;
    }

    public static float ClampMagnitude(float signedValue, float absoluteMagnitude){
        return Mathf.Clamp(signedValue, -absoluteMagnitude, absoluteMagnitude);
    }

    public static void GetOrthoBasis(Vector3 forward, out Vector3 up, out Vector3 right){
        up = Vector3.up;
        float howParallel = Mathf.Abs(Vector3.Dot(forward, up));
        Vector3 operand = ( howParallel < 0.9f ) ? up : Vector3.forward;

        Vector3 something = Vector3.Cross(forward, operand);
        up = Vector3.Cross(forward, something);
        right = Vector3.Cross(up, forward);
    }

    public static void GetFirstBranchComponents<C>(GameObject root, List<C> dstComponents) where C:Component {
        int childCount = root.transform.childCount;
        for(int i = 0; i < childCount; i++){
            var childTrans = root.transform.GetChild(i);
            C component = childTrans.GetComponent<C>();
            if(component){
                // Don't go any deeper.
                dstComponents.Add(component);
            } else {
                GetFirstBranchComponents(childTrans.gameObject, dstComponents);
            }
        }
    }

    public static Gradient TwoStopGradient(Color startColor, Color endColor, float startAlpha, float endAlpha){
        Gradient g = new Gradient();
        g.colorKeys = new GradientColorKey[2]{ new GradientColorKey(startColor, 0f), new GradientColorKey(endColor, 1f) };
        g.alphaKeys = new GradientAlphaKey[2]{ new GradientAlphaKey(startAlpha, 0f), new GradientAlphaKey(endAlpha, 1f) };
        return g;
    }

    public static Gradient SingleValue(Color c){
        return TwoStopGradient(c, c, c.a, c.a);
    }

    // 1,1 => 0
    // 1,0 => 1
    // 0,1 => -1,
    // 0,0 => 0
    public static int ParseBitDelta(int oldBitfield, int newBitfield, int singleBitmask) {
        int lhs = System.Convert.ToInt32((newBitfield&singleBitmask) != 0);
        int rhs = System.Convert.ToInt32((oldBitfield&singleBitmask) != 0);
        int res = lhs - rhs;
        return res;
    }

    public static System.Guid ParseGUIDWell(in string charString){
        // For some dumb reason, Unity's .NET system.guid constructor implementation
        // calls substring during construction.
        int a = ((int)ToLiteralByte(charString, 0) << 24)
            |   ((int)ToLiteralByte(charString, 2) << 16)
            |   ((int)ToLiteralByte(charString, 4) << 8)
            |   ((int)ToLiteralByte(charString, 6)
        );
        short b = (short)(((int)ToLiteralByte(charString, 8) << 8) | (int)ToLiteralByte(charString, 10));
        short c = (short)(((int)ToLiteralByte(charString, 12) << 8) | (int)ToLiteralByte(charString, 14));
        byte d = ToLiteralByte(charString, 16);
        byte e = ToLiteralByte(charString, 18);
        byte f = ToLiteralByte(charString, 20);
        byte g = ToLiteralByte(charString, 22);
        byte h = ToLiteralByte(charString, 24);
        byte i = ToLiteralByte(charString, 26);
        byte j = ToLiteralByte(charString, 28);
        byte k = ToLiteralByte(charString, 30);
        return new System.Guid(a, b, c, d, e, f, g, h, i, j, k);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToLiteralByte(in string literalByteString, int offset){
        int highOrderValue = ToLiteralHexValue(literalByteString[offset]);
        int lowOrderValue = ToLiteralHexValue(literalByteString[offset+1]);
        int wholeByte = (highOrderValue << 4) | lowOrderValue;
        return (byte)(wholeByte);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToLiteralHexValue(char hexAsChar){
        return (hexAsChar <= '9') ? (byte)(hexAsChar - '0') : (byte)(hexAsChar - 'a' + 0xa);
    }

    public static bool CrossesGTE(float oldValue, float newValue, float threshold){
        return (oldValue < threshold && threshold <= newValue);
    }

    public static bool CrossesPeriodicThreshold(float oldValue, float newValue, float period){
        float oldDivided = oldValue / period;
        float newDivided = newValue / period;
        return ( Mathf.FloorToInt(oldDivided) != Mathf.FloorToInt(newDivided) );
    }

    public static float Remap(float inOldValue, float oldMin, float oldMax, float newMin, float newMax) {
        return Mathf.Lerp( newMin, newMax, Mathf.InverseLerp(oldMin, oldMax, inOldValue) );
    }

    public static Vector3 ForceMagnitude(Vector3 vec, float magnitude){
        if(vec == Vector3.zero){
            return vec;
        }
        return vec.normalized * magnitude;
    }

    public static Vector3 Remap( Vector3 inOldValue, Vector3 oldMin, Vector3 oldMax, Vector3 newMin, Vector3 newMax ){
        return Lerp3( newMin, newMax, InverseLerp(oldMin, oldMax, inOldValue) );
    }

    public static Vector3 InverseLerp(Vector3 a, Vector3 b, Vector3 value){
        return new Vector3(
            Mathf.InverseLerp(a.x, b.x, value.x),
            Mathf.InverseLerp(a.y, b.y, value.y),
            Mathf.InverseLerp(a.z, b.z, value.z)
        );
    }

    public static Vector3 Lerp3(Vector3 a, Vector3 b, Vector3 t){
        return new Vector3(
            Mathf.Lerp(a.x, b.x, t.x),
            Mathf.Lerp(a.y, b.y, t.y),
            Mathf.Lerp(a.z, b.z, t.z)
        );
    }

    public static Vector4 Multiply(Vector4 a, Vector4 b){
        return new Vector4(a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w);
    }

    public static Vector3 Multiply(Vector3 a, Vector3 b){
        return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z);
    }

    public static Vector2 Multiply(Vector2 a, Vector2 b){
        return new Vector2(a.x * b.x, a.y*b.y);
    }

    public static Vector4 Divide(Vector4 a, Vector4 b){
        return new Vector4(a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w);
    }

    public static Vector3 Divide(Vector3 a, Vector3 b){
        return new Vector3(a.x/b.x, a.y/b.y, a.z/b.z);
    }

    public static Vector2 Divide(Vector2 a, Vector2 b){
        return new Vector2(a.x / b.x, a.y/b.y);
    }

    public static void DrawCircle(Vector3 center, Vector3 up, float radius, Color color){
        Vector3 arm = Mathf.Abs(Vector3.Dot(up, Vector3.up)) < 0.95f ? up : Vector3.back;
        int segmentCount = 32;
        float angleDelta = 360f / segmentCount;
        for(int i = 0; i < segmentCount-1; i++){
            int j = i+1;
            float angleA = i * angleDelta;
            float angleB = j * angleDelta;
            Vector3 pointA = center + radius * (Quaternion.AngleAxis(angleA, up) * arm);
            Vector3 pointB = center + radius * (Quaternion.AngleAxis(angleB, up) * arm);
            Debug.DrawLine(pointA, pointB, color);
        }
    }

    public static bool TestPointInPlanes(Vector3 point, Plane[] planes){
        foreach(var plane in planes){
            if(plane.GetSide(point) == false){
                return false;
            }
        }
        return true;
    }

    public static T GetClosestComponentFromAncestors<T>(Transform user) where T:UnityEngine.Component{
        if(user.TryGetComponent(out T component)){
            return component;
        } else {
            if(user.parent){
                return GetClosestComponentFromAncestors<T>(user.parent);
            } else {
                return null;
            }
        }
    }

}

}
