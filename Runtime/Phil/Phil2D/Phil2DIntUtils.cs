using UnityEngine;

namespace Phil {

public static class Phil2DIntUtils {

    public static RectInt ToWorldPixelRect(Vector2 worldPivotPosition, Sprite sprite){
        var pivotPos_px = sprite.pixelsPerUnit * worldPivotPosition;
        var pivot_px = sprite.pivot;
        var rectSize_px = sprite.rect.size;
        return new RectInt( Vector2Int.RoundToInt(pivotPos_px - pivot_px), Vector2Int.RoundToInt(rectSize_px) );
    }

    public static RectInt ToBoundingBox(Vector2Int start, Vector2Int delta){
        var end = start + delta;
        var min = Vector2Int.Min(start,end);
        var size = new Vector2Int( Mathf.Abs(delta.x), Mathf.Abs(delta.y) );
        return new RectInt(min, size);
    }

    public static RectInt ToBoundingBox(RectInt start, Vector2Int delta){
        var end = new RectInt(start.position + delta, start.size);
        var min = Vector2Int.Min( end.position, start.position );
        var size = new Vector2Int( Mathf.Abs(delta.x), Mathf.Abs(delta.y) ) + start.size;
        return new RectInt(min, size);
    }

    // TODO: mimic logical & operation from Phil.Core.RectInt
    public static bool TryGetRectIntersection(RectInt a, RectInt b, out RectInt intersection){
        if (!a.Overlaps (b)) {
			intersection = new RectInt(0,0,0,0);
            return false;
		} else {
            var aMax = a.TrueMax();
            var bMax = b.TrueMax();
			int maxLeft = Mathf.Max (a.xMin, b.xMin);
			int minRight = Mathf.Min (aMax.x, bMax.x);
			int minTop = Mathf.Min (aMax.y, bMax.y);
			int maxBot = Mathf.Max (a.yMin, b.yMin);
			int width = minRight - maxLeft + 1;
			int height = minTop - maxBot + 1;
			intersection = new RectInt ( maxLeft, maxBot, width, height );
            return true;
		}
    }

    public static bool RectIntCast(RectInt origin, Vector2Int delta, RectInt obstruction, 
        out RectInt dst, out Vector2Int contactNormal, out RectInt overlapper)
    {
        // TODO: bounding-box check...
        if(delta == Vector2Int.zero){
            dst = origin;
            contactNormal = Vector2Int.zero;
            overlapper = new RectInt();
            return false;
        }

        float strideWidth = (float)origin.width / 2;
        float strideHeight = (float)origin.height / 2;
        int slices = Mathf.Max( 
            Mathf.CeilToInt((float)Mathf.Abs(delta.x)/strideWidth), 
            Mathf.CeilToInt((float)Mathf.Abs(delta.y)/strideHeight) 
        );

        Vector2 floatStride = new Vector2(delta.x,delta.y) / slices;
        // Debug.LogFormat("float stride: {0}", floatStride);
        RectInt? overlapRect = null;
        RectInt coarseRect = new RectInt(0,0,0,0);

        for(int i = 0; i < slices+1; i++){
            Vector2Int position = origin.position + Vector2Int.RoundToInt(i * floatStride);
            RectInt iterRect = new RectInt(position, origin.size);

            // Debug.LogFormat("iter rect: {0}", iterRect);
        
            if(iterRect.Overlaps(obstruction)){
                TryGetRectIntersection(iterRect, obstruction, out RectInt ol);
                overlapRect = ol;
                coarseRect = iterRect;
                break;
            }
        }

        if(overlapRect.HasValue == false){
            dst = new RectInt(origin.position + delta, origin.size);
            contactNormal = Vector2Int.zero;
            overlapper = new RectInt();
            return false;
        }
        var overlap = overlapRect.Value;
        overlapper = overlap;
        // Debug.LogFormat("Coarse rect: {0}", coarseRect);
        // Debug.LogFormat("Overlap rect: {0}", overlap);


        // At this point, one of our rects overlapped
        // Use our delta, coarse rect, and overlap rect to back-out of overlapping
        Vector2Int backTrack = Vector2Int.zero;

        // TODO: fix contact normal
        contactNormal = Vector2Int.zero;
        // Vector2Int backTrackA = Vector2Int.zero;
        if(delta.x != 0){
            float slope = (float)delta.y / delta.x;
            if(-delta.x > 0){
                int rightResolve = 1 + overlap.TrueMax().x - coarseRect.x;
                // Debug.LogFormat("Right resolve: {0}", rightResolve);
                backTrack.x = rightResolve;
                backTrack.y = Mathf.RoundToInt(rightResolve * slope);
                contactNormal = Vector2Int.right;
            } else {
                int leftResolve = -Mathf.Abs(coarseRect.TrueMax().x - overlap.x + 1);
                // Debug.LogFormat("Left resolve: {0}", leftResolve);
                backTrack.x = leftResolve;
                backTrack.y = Mathf.RoundToInt(leftResolve * slope);
                contactNormal = Vector2Int.left;
            }
        }
        // Vector2Int backTrackB = Vector2Int.zero;
        if(delta.y != 0) {
            // Pure vertical
            float invSlope = (float)delta.x / delta.y;
            if(delta.y > 0){
                int downResolve = -Mathf.Abs(coarseRect.TrueMax().y - overlap.y + 1);
                // Debug.LogFormat("Down resolve: {0}", downResolve);
                backTrack.y = downResolve;
                backTrack.x = Mathf.RoundToInt(downResolve * invSlope);
                contactNormal = Vector2Int.down;
            } else {
                int upResolve = 1 + overlap.TrueMax().y - coarseRect.y;
                // Debug.LogFormat("Up resolve: {0}", upResolve);
                backTrack.y = upResolve;
                backTrack.x = Mathf.RoundToInt(upResolve * invSlope);
                contactNormal = Vector2Int.up;
            }
        }

        // Note: we don't handle corner-to-corner collisions here.

        Vector2Int finalRectPos = coarseRect.position + backTrack;
        dst = new RectInt(finalRectPos, origin.size);
        // Debug.LogFormat("Final rect: {0}", finalRectPos);
        return true;
    }

    public static bool RayRectIntersect(Vector2Int origin, Vector2Int delta, RectInt rect, out Vector2Int preContact, out Vector2Int contactNormal){
        // # Bounding Box Check
        Vector2Int raySize = Vector2Int.Max( Vector2Int.one, new Vector2Int(Mathf.Abs(delta.x), Mathf.Abs(delta.y)) );
        Vector2Int rayDst = origin + delta;
        Vector2Int rayCorner = Vector2Int.Min(origin, origin + delta);
        RectInt rayBoundingBox = new RectInt(rayCorner.x, rayCorner.y, raySize.x, raySize.y);

        if(rayBoundingBox.Overlaps(rect) == false) {
            contactNormal = Vector2Int.zero;
            preContact = Vector2Int.zero;
            return false;
        }

        // Wall Checks
        Vector2Int? southHit = RayOrthoLineIntersect(origin, delta, rect.position, true, rect.width);
        Vector2Int? northHit = RayOrthoLineIntersect(origin, delta, rect.max, true, -rect.width);
        Vector2Int? westHit = RayOrthoLineIntersect(origin, delta, rect.position, false, rect.height);
        Vector2Int? eastHit = RayOrthoLineIntersect(origin, delta, rect.max, false, -rect.height);

        Vector2Int? NSCloser = GetCloser(origin, southHit, northHit);
        Vector2Int? EWCloser = GetCloser(origin, westHit, eastHit);
        Vector2Int? closest = GetCloser(origin, NSCloser, EWCloser);

        preContact = closest ?? Vector2Int.zero;
        if(closest.HasValue){
            if(closest == southHit){
                contactNormal = Vector2Int.down;
            } else if(closest == northHit){
                contactNormal = Vector2Int.up;
            } else if(closest == westHit){
                contactNormal = Vector2Int.left;
            } else {
                contactNormal = Vector2Int.right;
            }
        } else {
            contactNormal = Vector2Int.zero;
        }
        return closest.HasValue;
    }

    private static Vector2Int? GetCloser(Vector2Int origin, Vector2Int? a, Vector2Int? b){
        if(a.HasValue == b.HasValue){
            if(a.HasValue == false){
                // They're both false
                return null;
            }
            var toA = a.Value - origin;
            var toB = b.Value - origin;
            int toASquareMag = (toA.x*toA.x) + (toA.y*toA.y);
            int toBSquareMag = (toB.x*toB.x) + (toB.y*toB.y);
            return (toASquareMag < toBSquareMag) ? a : b;
        } else {
            Vector2Int validValue = a ?? b.Value;
            return validValue;
        }
    }

    public static Vector2Int? RayOrthoLineIntersect(Vector2Int rayOrigin, Vector2Int delta, Vector2Int lineStart, bool horzTrueVertFalse, int linePixelLength){
        if( RayOrthoLineIntersect(rayOrigin, delta, lineStart, horzTrueVertFalse, linePixelLength, out Vector2Int contactPoint) ){
            return contactPoint;
        } else {
            return null;
        }
    }

    public static bool RayOrthoLineIntersect(Vector2Int rayOrigin, Vector2Int delta, Vector2Int lineStart, bool horzTrueVertFalse, int linePixelLength, out Vector2Int contactPoint){
        Vector2 floatRayOrigin = new Vector2(rayOrigin.x, rayOrigin.y);
        Vector2 floatRayDelta = new Vector2(delta.x, delta.y);
        Vector2 floatRayEnd = floatRayOrigin + floatRayDelta;
        
        Vector2 floatLineStart = new Vector2(lineStart.x, lineStart.y);
        Vector2 floatLineEnd = floatLineStart + (linePixelLength-1) * (horzTrueVertFalse ? Vector2.right : Vector2.up);
        Vector2 trueLineStart = Vector2.Min(floatLineStart, floatLineEnd);
        Vector2 trueLineEnd = Vector2.Max(floatLineStart, floatLineEnd);

        Vector2Int absDelta = new Vector2Int( Mathf.Abs(delta.x), Mathf.Abs(delta.y) );

        // Bloat this by one unit to simulate a solid wall:
        if(horzTrueVertFalse){
            bool rayBelow = (rayOrigin.y < lineStart.y);
            trueLineStart += rayBelow ? new Vector2(-1,-1) : new Vector2(-1, 1);
            trueLineEnd += rayBelow ? new Vector2(1,-1) : new Vector2(1,1);
        } else {
            bool rayFromTheLeft = (rayOrigin.x < lineStart.x);
            trueLineStart += rayFromTheLeft ? new Vector2(-1,-1) : new Vector2(1,-1);
            trueLineEnd += rayFromTheLeft ? new Vector2(-1, 1) : new Vector2(1,1);
        }

        if(Phil2DUtils.TryIntersectLineSegments(floatRayOrigin, floatRayEnd, trueLineStart, trueLineEnd, out Vector2 bloatedIntersection)){
            contactPoint = Vector2Int.RoundToInt(bloatedIntersection);
            return true;
        } else {
            contactPoint = Vector2Int.zero;
            return false;
        }
    }

}

}