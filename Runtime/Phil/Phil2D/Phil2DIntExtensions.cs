using UnityEngine;

namespace Phil {

public static class Phil2DIntExtensions {

    public static Vector2 ToVector2(this Vector2Int v2i){
        return new Vector2(v2i.x, v2i.y);
    }

    public static Vector2 GetScaled(this Vector2Int v2i, float f){
        return new Vector2( v2i.x * f, v2i.y * f);
    }

    public static Vector2Int XY(this Vector3Int v3i){ return new Vector2Int(v3i.x, v3i.y); }

    public static Vector2Int TrueMax(this RectInt rectInt){
        return rectInt.max - Vector2Int.one;
    }

    public static Vector2Int CeiledCenter(this RectInt rectInt){
        int halfWidthCeil = Mathf.CeilToInt((float)rectInt.width / 2);
        int halfHeightCeil = Mathf.CeilToInt((float)rectInt.height / 2);
        Vector2Int center = rectInt.position + new Vector2Int(halfWidthCeil, halfHeightCeil);
        return center;
    }

    public static RectInt SetTrueMinMax(ref this RectInt ri, Vector2Int min, Vector2Int max){
        var size = max-min+Vector2Int.one;
        ri = new RectInt(min, size);
        return ri;
    }

    public static Rect ToRect(this RectInt ri){
        return new Rect(ri.position.x, ri.position.y, ri.width, ri.height);
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

    public static Vector2Int Clamp(this RectInt ri, Vector2Int coord){
        var tm = ri.TrueMax();
        coord.Clamp(ri.min, tm);
        return coord;
    }

    public static int Left(this RectInt rectInt){
        return rectInt.xMin;
    }

    public static int Right(this RectInt rectInt){
        return rectInt.xMax - 1;
    }

    public static int Top(this RectInt rectInt){
        return rectInt.yMax - 1;
    }

    public static int Bottom(this RectInt rectInt){
        return rectInt.yMin;
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

    public static RectInt GetScaledAboutPivot(this RectInt rectInt, Vector2Int pivot, Vector2 scale){
        var trueMax = rectInt.TrueMax();
        var toMax = trueMax - pivot;
        var toMin = rectInt.min - pivot;
        toMax = Vector2Int.RoundToInt( new Vector2(toMax.x*scale.x, toMax.y*scale.y) );
        toMin = Vector2Int.RoundToInt( new Vector2(toMin.x*scale.x, toMin.y*scale.y) );
        var size = toMax-toMin+Vector2Int.one;
        var position = pivot + toMin;
        return new RectInt(position, size);
    }

    static Vector2Int GetPreMirrorDiff(Vector2Int target, Vector2Int pivot){
        Vector2Int pmd = new Vector2Int(
            (target.x >= pivot.x) ? (target.x - pivot.x + 1) : (target.x - pivot.x),
            (target.y >= pivot.y) ? (target.y - pivot.y + 1) : (target.y - pivot.y)
        );
        return pmd;
    }

    public static int GetSpiralFlatIndex(this Vector2Int center){
        int ringNumber = Mathf.Max( Mathf.Abs(center.x), Mathf.Abs(center.y) );
        int twoRN = 2*ringNumber;
        int squareSide = (twoRN) + 1;
        
        int quant = (2*(ringNumber-1)) + 1;
        int northIndex = quant * quant;
        int northEastIndex = northIndex + ringNumber;
        int southEastIndex = northEastIndex + twoRN;
        int southWestIndex = southEastIndex + twoRN;
        int northWestIndex = southWestIndex + twoRN;

        if(center.x == ringNumber){
            // East wall
            Vector2Int northEast = Vector2Int.zero.SpiralOut(northEastIndex);
            int downwards = Mathf.Abs( center.y - northEast.y );
            return northEastIndex + downwards;
        } else if(center.x == -ringNumber){
            // West wall
            Vector2Int southWest = Vector2Int.zero.SpiralOut(southWestIndex);
            int upwards = center.y - southWest.y;
            return southWestIndex + upwards;
        } else if(center.y == ringNumber){
            // North wall
            Vector2Int north = Vector2Int.zero.SpiralOut(northIndex);
            Vector2Int northWest = Vector2Int.zero.SpiralOut(northWestIndex);
            return (center.x >= north.x) ? (center.x - north.x) + northIndex : (center.x - northWest.x) + northWestIndex;
        } else { // center.y == -ringNumber
            // South wall
            Vector2Int southEast = Vector2Int.zero.SpiralOut(southEastIndex);
            int westward = Mathf.Abs( center.x - southEast.x );
            return southEastIndex + westward;
        }
    }

    public static Vector2Int SpiralOut(this Vector2Int center, int flatIndex){
        if (flatIndex <= 0)
			return center;

		switch (flatIndex) {
		case 1:
			return center + Vector2Int.up;
		case 2:
			return center + Vector2Int.one;
		case 3:
			return center + Vector2Int.right;
		case 4:
			return center + new Vector2Int (1, -1);
		case 5:
			return center + Vector2Int.down;
		case 6:
			return center - Vector2Int.one;
		case 7:
			return center + Vector2Int.left;
		case 8:
			return center + new Vector2Int (-1, 1);
		default:
			break;
		}

		int lowerSqrt = Mathf.FloorToInt (Mathf.Sqrt ((float)flatIndex));
		int oddLowerSqrt = (lowerSqrt % 2 == 0) ? lowerSqrt - 1 : lowerSqrt;

		int distFromCenter = (oddLowerSqrt/2) + 1; // 1->1; 3->2; 5->3; 7->4; ...

		int minValueOnRing = oddLowerSqrt*oddLowerSqrt; // one to the right of the northwest corner
		int maxValueOnRing = (oddLowerSqrt+2) * (oddLowerSqrt+2) -1; // nw corner
		int ringSideLength = 2 * distFromCenter + 1;

		int NE_post = minValueOnRing + 2*distFromCenter - 1;
		int SE_post = NE_post + ringSideLength - 1;
		int SW_post = SE_post + ringSideLength - 1;
		int NW_post = SW_post + ringSideLength - 1;

		if(flatIndex == minValueOnRing){
            var value = center + new Vector2Int(-1,1) * distFromCenter + Vector2Int.right;
            // Debug.LogFormat("flat index: {0}, dist form center: {1}, value: {2}, center: {3}", flatIndex, distFromCenter, value, center);
			return value;
        }
		if(flatIndex == NE_post)
			return center + Vector2Int.one * distFromCenter;
		if(flatIndex == SE_post)
			return center + new Vector2Int (1, -1) * distFromCenter;
		if (flatIndex == SW_post)
			return center - Vector2Int.one * distFromCenter;
		if (flatIndex == NW_post) // AKA, max value on ring
			return center + new Vector2Int (-1, 1) * distFromCenter;


		// Somewhere in between....
		if (flatIndex > NE_post && flatIndex < SE_post) {
			// East edge.
			Vector2Int NE =  center + Vector2Int.one * distFromCenter;
			int diff = flatIndex - NE_post;
			return NE + Vector2Int.down * diff;
		}
		if (flatIndex > SE_post && flatIndex < SW_post) {
			// South edge
			Vector2Int SE = center + new Vector2Int (1, -1) * distFromCenter;
			int diff = flatIndex - SE_post;
			return SE + Vector2Int.left * diff;
		}
		if (flatIndex > SW_post && flatIndex < NW_post) {
			// West edge
			Vector2Int SW = center - Vector2Int.one * distFromCenter;
			int diff = flatIndex - SW_post;
			return SW + Vector2Int.up * diff;
		}
		if (flatIndex > minValueOnRing && flatIndex < NE_post) {
			// North edge
            Vector2Int NW = center + new Vector2Int (-1, 1) * distFromCenter;
			int diff = flatIndex - minValueOnRing + 1;
            var v = NW + Vector2Int.right * diff;
            // Debug.Log(v);
			return v;
		}
			
		Debug.LogErrorFormat("You fucked up. Flat index: {0}. Posts: NE {1}, SE {2}, SW {3}, NW: {4}",
            flatIndex, NE_post, SE_post, SW_post, NW_post
        );
		return center;
    }

}

}
