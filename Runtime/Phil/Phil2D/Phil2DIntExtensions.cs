using UnityEngine;

namespace Phil {

public static class Phil2DIntExtensions {

    public static Vector2 ToVector2(this Vector2Int v2i){
        return new Vector2(v2i.x, v2i.y);
    }

    public static Vector2 GetScaled(this Vector2Int v2i, float f){
        return new Vector2( v2i.x * f, v2i.y * f);
    }

    public static Vector2Int TrueMax(this RectInt rectInt){
        return rectInt.max - Vector2Int.one;
    }

    public static RectInt SetTrueMinMax(ref this RectInt ri, Vector2Int min, Vector2Int max){
        var size = max-min+Vector2Int.one;
        ri = new RectInt(min, size);
        return ri;
    }

    public static int Area(this RectInt rectInt){
        return rectInt.width * rectInt.height;
    }

    public static Vector2Int GetCoord(this RectInt rectInt, int flatIndex){
		flatIndex = Mathf.Clamp (flatIndex, 0, rectInt.Area() - 1);

		int xCoord = rectInt.position.x + (flatIndex % rectInt.width);
		int yCoord = rectInt.position.y + (flatIndex / rectInt.width);

		return new Vector2Int (xCoord, yCoord);
	}

    public static RectInt GetMirroredRectInt(this RectInt rectInt, Vector2Int pivot, bool horzMirror, bool vertMirror){
        var rectIntTrueMax = rectInt.TrueMax();
        Vector2Int toTopRight = GetPreMirrorDiff(rectIntTrueMax, pivot);
        Vector2Int toBotLeft = GetPreMirrorDiff(rectInt.position, pivot);

        if(horzMirror){
            toTopRight.x *= -1;
            toBotLeft.x *= -1;
        }
        if(vertMirror){
            toTopRight.y *= -1;
            toBotLeft.y *= -1;
        }

        if(toTopRight.x > 0){
            toTopRight.x--;
        }
        if(toTopRight.y > 0){
            toTopRight.y--;
        }
        if(toBotLeft.x > 0){
            toBotLeft.x--;
        }
        if(toBotLeft.y > 0){
            toBotLeft.y--;
        }

        Vector2Int pointA = pivot + toTopRight;
        Vector2Int pointB = pivot + toBotLeft;

        Vector2Int min = Vector2Int.Min(pointA, pointB);
        Vector2Int max = Vector2Int.Max(pointA, pointB);

        RectInt ri = new RectInt();
        ri.SetTrueMinMax(min,max);
        return ri;
    }

    static Vector2Int GetPreMirrorDiff(Vector2Int target, Vector2Int pivot){
        Vector2Int pmd = new Vector2Int(
            (target.x >= pivot.x) ? (target.x - pivot.x + 1) : (target.x - pivot.x),
            (target.y >= pivot.y) ? (target.y - pivot.y + 1) : (target.y - pivot.y)
        );
        return pmd;
    }

}

}
