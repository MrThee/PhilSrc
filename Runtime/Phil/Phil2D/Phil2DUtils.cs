using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Phil {

public static class Phil2DUtils {

    public static Vector2 GetLossySize(this BoxCollider2D boxCollider){
        return Vector2.Scale( boxCollider.transform.lossyScale, boxCollider.size );
    }
    
    public static Vector2 GetWorldCenter(this BoxCollider2D boxCollider){
        Vector2 boxPos = boxCollider.transform.position + Vector3.Scale( boxCollider.transform.lossyScale, boxCollider.offset );
        return boxPos;
    }

    public static Vector2 SnapToPixelGrid(Vector2 position, int pixelsPerUnit){
        return new Vector2(
            Mathf.RoundToInt(position.x*pixelsPerUnit) * (1f/pixelsPerUnit),
            Mathf.RoundToInt(position.y*pixelsPerUnit) * (1f/pixelsPerUnit)
        );
    }

    // https://github.com/setchi/Unity-LineSegmentsIntersection/blob/master/Assets/LineSegmentIntersection/Scripts/Math2d.cs
    public static bool TryIntersectLineSegments(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection){
        intersection = Vector2.zero;
        
        Vector2 cd = d-c;
        Vector2 ab = b-a;
        Vector2 ac = c-a;

        var denom = ab.x * cd.y - ab.y * cd.x;

        if (denom == 0.0f)
        {
            return false;
        }

        var u = (ac.x * cd.y - ac.y * cd.x) / denom;
        var v = (ac.x * ab.y - ac.y * ab.x) / denom;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = a.x + u * ab.x;
        intersection.y = a.y + u * ab.y;

        return true;
    }

}

}