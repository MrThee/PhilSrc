using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Phil {

    public static class PhilExtensions{

        // int
        public static int Sign(this int i){
            if(i > 0){ 
                return 1;
            } else if (i == 0){
                return 0;
            } else {
                return -1;
            }
        }

        // float
        public static int Sign(this float i){
            if(i > 0f){
                return 1;
            } else if(i == 0){
                return 0;
            } else {
                return -1;
            }
        }

        // bool
        public static int AxisSign(this bool b){
            return b ? 1 : -1;
        }

        // Value Tuples
        public static bool Contains<I>(this (I,I) pair, I item) where I:System.IEquatable<I>{
            return item.Equals(pair.Item1) || item.Equals(pair.Item2);
        }

        public static bool Contains<I>(this (I,I,I) triple, I item) where I:System.IEquatable<I>{
            return item.Equals(triple.Item1) || item.Equals(triple.Item2) || item.Equals(triple.Item3);
        }

        public static bool Contains<I>(this (I,I,I,I) quad, I item) where I:System.IEquatable<I>{
            return item.Equals(quad.Item1) || item.Equals(quad.Item2) || item.Equals(quad.Item3) || item.Equals(quad.Item4);
        }

        public static bool Contains<I>(this (I,I,I,I,I,I,I,I) oct, I item) where I:System.IEquatable<I>{
            return item.Equals(oct.Item1) || item.Equals(oct.Item2) || item.Equals(oct.Item3) || item.Equals(oct.Item4)
                || item.Equals(oct.Item5) || item.Equals(oct.Item6) || item.Equals(oct.Item7) || item.Equals(oct.Item8)
            ;
        }
        public static T GetItem<T>(this (T,T,T) quad, int i){
            switch(i){
            default: throw new System.ArgumentException(string.Format("Bad index: {0}", i));
            case 0: return quad.Item1;
            case 1: return quad.Item2;
            case 2: return quad.Item3;
            }
        }

        public static void SetItem<T>(ref this (T,T,T) quad, int i, T value){
            switch(i){
            default: throw new System.ArgumentException(string.Format("Bad index: {0}", i));
            case 0: quad.Item1 = value; break;
            case 1: quad.Item2 = value; break;
            case 2: quad.Item3 = value; break;
            }
        }

        public static T GetItem<T>(this (T,T,T,T) quad, int i){
            switch(i){
            default: throw new System.ArgumentException(string.Format("Bad index: {0}", i));
            case 0: return quad.Item1;
            case 1: return quad.Item2;
            case 2: return quad.Item3;
            case 3: return quad.Item4;
            }
        }

        public static void SetItem<T>(ref this (T,T,T,T) quad, int i, T value){
            switch(i){
            default: throw new System.ArgumentException(string.Format("Bad index: {0}", i));
            case 0: quad.Item1 = value; break;
            case 1: quad.Item2 = value; break;
            case 2: quad.Item3 = value; break;
            case 3: quad.Item4 = value; break;
            }
        }

        // Dictionary.KeyValue
        public static (K,V) ToDuple<K,V>(this System.Collections.Generic.KeyValuePair<K,V> keyValuePair){
            return (keyValuePair.Key, keyValuePair.Value);
        }

        public static void RemoveEntries<K,V>(this Dictionary<K,V> dict, IReadOnlyList<K> keys){
            int keyCount = keys.Count;
            for(int i = 0; i < keyCount; i++){
                dict.Remove(keys[i]);
            }
        }

        // System.Array
        public static bool IsNullOrEmpty<T>( this T[] arr ){
            return (arr == null || arr.Length == 0);
        }

        // returns the new valid size of the array
        public static int RemoveAtSansResizing<T>( this T[] arr, int index){
            int L = arr.Length;
            for(int i = index; i < L-1; i++){
                arr[i] = arr[i+1];
            }
            return L-1;
        }

        // System.Nullable
        public static bool TryGetValue<T>(this System.Nullable<T> nullable, out T value) where T:struct {
            if(nullable.HasValue){
                value = nullable.Value;
                return true;
            } else {
                value = default(T);
                return false;
            }
        }

        // String Builder
        public static System.Text.StringBuilder RemoveFirstLine(this System.Text.StringBuilder sb){
            int chars2Remove = 0;
            int size = sb.Length;
            for(chars2Remove = 0; chars2Remove < size; chars2Remove++){
                if(sb[chars2Remove] == '\n'){
                    chars2Remove++;
                    break;
                }
            }
            sb.Remove(0, chars2Remove);
            return sb;
        }

        public static int NonWhitespaceCount(this System.Text.StringBuilder sb){
            int bufLen = sb.Length;
            int nonWhiteCount = 0;
            for(int i = 0; i < bufLen; i++){
                if(false == char.IsWhiteSpace(sb[i])){
                    nonWhiteCount++;
                }
            }
            return nonWhiteCount;
        }

        public static void CopyFrom(this System.Text.StringBuilder dst, System.Text.StringBuilder src){
            int srcLen = src.Length;
            dst.Clear();
            for(int i = 0; i < srcLen; i++){
                dst.Append(src[i]);
            }
        }

        // Rect
        public static Vector2 RemapWithin(this Rect rect, Vector2 pointWithin){
            Vector2 fromMin = pointWithin - rect.min;
            return Utils.Divide( fromMin, rect.size );
        }

        public static Vector2 ClampPoint(this Rect rect, Vector2 point){
            float x = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
            float y = Mathf.Clamp(point.y, rect.yMin, rect.yMax);
            return new Vector2(x,y);
        }

        public static bool Intersect(this Rect a, Rect b, out Rect c){
            if(!a.Overlaps(b)){
                c = new Rect(0,0,0,0);
                return false;
            } else {
                var minOfMaxes = Vector2.Min(a.max, b.max);
                var maxOfMins = Vector2.Max(a.min, b.min);
                var size = minOfMaxes - maxOfMins;
                c = new Rect(maxOfMins, size);
                return true;
            }
        }

        // Plane
        public static bool Raycast(this Plane plane, Ray ray, out Vector3 intersection){
            if(plane.Raycast(ray, out float enter)){
                intersection = ray.origin + (ray.direction * enter);
                return true;
            } else {
                intersection = Vector3.zero;
                return false;
            }
        }

        // Vector3
        public static string BetterToString(this Vector3 vector3){
            return string.Format("x: {0}, y: {1}, z: {2}", vector3.x, vector3.y, vector3.z);
        }

        public static Vector3 Lateral(this Vector3 vector3){
            return new Vector3(vector3.x, 0f, vector3.z);
        }

        public static void NormAndMag(this Vector3 vector3, out Vector3 normalized, out float magnitude){
            normalized = vector3.normalized;
            magnitude = Vector3.Dot(normalized, vector3);
        }

        public static Vector3 CalcClampedMagnitude(this Vector3 vector3, float minMagnitude, float maxMagnitude){
            if(vector3 == Vector3.zero){
                Debug.LogWarning("Can't clamp the magnitude of a zero-vector. Returning Vector3.zero");
                return vector3;
            }
            Vector3 dir = vector3.normalized;
            float length = vector3.magnitude;
            float newLength = Mathf.Clamp(length, minMagnitude, maxMagnitude);
            return dir * newLength;
        }

        public static Vector3 CalcWithMinMagnitude(this Vector3 vector3, float minMagnitude){
            if(vector3 == Vector3.zero){
                Debug.LogWarning("Can't raise the magnitude of a zero-vector. Returning Vector3.zero");
                return vector3;
            }
            Vector3 dir = vector3.normalized;
            float curLength = vector3.magnitude;
            float newLength = Mathf.Max(minMagnitude, curLength);
            return dir * newLength;
        }

        public static Vector3 CalcConicallyClamped(this Vector3 v3, Vector3 originAxis, float maxAngle){
            float angleTo = Vector3.Angle(originAxis, v3);
            if(angleTo <= maxAngle){
                return v3;
            } else {
                Vector3 rotAxis = Vector3.Cross(originAxis, v3);
                return Quaternion.AngleAxis(maxAngle, rotAxis) * (originAxis.normalized * v3.magnitude);
            }
        }

        public static void CalcNormAndTanComponents(this ref Vector3 v3, Vector3 planeNormal, 
            out Vector3 normalComponent, out Vector3 tangentComponent)
        {
            normalComponent = Vector3.Project(v3, planeNormal);
            tangentComponent = Vector3.ProjectOnPlane(v3, planeNormal);
        }

        // Bounds
        public static Vector3 CalcNormalizedPoint(this Bounds bounds, Vector3 pointInBounds){
            Vector3 min = bounds.min;
            Vector3 fromMin = pointInBounds - min;
            return new Vector3(
                (bounds.size.x > 0) ? fromMin.x / bounds.size.x : 0f,
                (bounds.size.y > 0) ? fromMin.y / bounds.size.y : 0f,
                (bounds.size.z > 0) ? fromMin.z / bounds.size.z : 0f
            );
        }

        // Quaternion
        public static Quaternion RelativeTo(this Quaternion b, Quaternion a){
            return Quaternion.Inverse(a) * b;
        }

        // Color
        public static Color AlphaZero(this Color c){
            return c.WithAlpha(0f);
        }

        public static Color WithAlpha(this Color c, float alpha){
            return new Color(c.r, c.g, c.b, alpha);
        }

        // List
        public static bool IsNullOrEmpty<T>(this List<T> list){
            return (list==null || list.Count==0);
        }

        public static void MaybeExpandThenPopulate<T>(this List<T> list, int newCapacity, T newEntryValue){
            if(newCapacity <= list.Capacity){
                return;
            }
            list.Capacity = newCapacity;
            int firstIndex = list.Count;
            for(int i = firstIndex; i < list.Capacity; i++){
                list.Add(newEntryValue);
            }
        }

        public static T GetHighestScoringElement<T>(this List<T> list, System.Func<T, int> ScoreFunc) {
            int maxScore = int.MinValue;
            int bestIndex = 0;
            int listCount = list.Count;
            for(int i = 0; i < listCount; i++){
                int curScore = ScoreFunc(list[i]);
                if(curScore >= maxScore){
                    maxScore = curScore;
                    bestIndex = i;
                }
            }
            return list[bestIndex];
        }

        public static T GetLowestScoringElement<T>(this List<T> list, System.Func<T, int> ScoreFunc) {
            int minScore = int.MaxValue;
            int bestIndex = 0;
            int listCount = list.Count;
            for(int i = 0; i < listCount; i++){
                int curScore = ScoreFunc(list[i]);
                if(curScore <= minScore){
                    minScore = curScore;
                    bestIndex = i;
                }
            }
            return list[bestIndex];
        }

        public static T GetHighestScoringElement<T>(this List<T> list, System.Func<T, float> ScoreFunc) {
            float maxScore = Mathf.NegativeInfinity;
            int bestIndex = 0;
            int listCount = list.Count;
            for(int i = 0; i < listCount; i++){
                float curScore = ScoreFunc(list[i]);
                if(curScore >= maxScore){
                    maxScore = curScore;
                    bestIndex = i;
                }
            }
            return list[bestIndex];
        }

        public static T GetLowestScoringElement<T>(this List<T> list, System.Func<T, float> ScoreFunc) {
            float minScore = Mathf.Infinity;
            int bestIndex = 0;
            int listCount = list.Count;
            for(int i = 0; i < listCount; i++){
                float curScore = ScoreFunc(list[i]);
                if(curScore <= minScore){
                    minScore = curScore;
                    bestIndex = i;
                }
            }
            return list[bestIndex];
        }

        public static T RollRandomElement<T>(this List<T> list){
            int ri = UnityEngine.Random.Range(0, list.Count);
            return list[ri];
        }

        // Animation Curve
        public static AnimationCurve Clone(this AnimationCurve curve){
            Keyframe[] copyFrames = new Keyframe[curve.keys.Length];
            curve.keys.CopyTo(copyFrames, 0);
            
            AnimationCurve copyCurve = new AnimationCurve(copyFrames);
            copyCurve.preWrapMode = curve.preWrapMode;
            copyCurve.postWrapMode = curve.postWrapMode;
            return copyCurve;
        }

        // Transform
        public static void Reset(this Transform transform){
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void CopyLocalTRS(this Transform transform, Transform rhs){
            transform.localPosition = rhs.localPosition;
            transform.localRotation = rhs.localRotation;
            transform.localScale = rhs.localScale;
        }

        public static void CopyWorldOrientation(this Transform transform, Transform rhs){
            transform.position = rhs.position;
            transform.rotation = rhs.rotation;
        }

        // Rect Transform
        public static void Embed(this RectTransform rTrans){
            rTrans.anchorMin = Vector2.zero;
            rTrans.anchorMax = Vector2.one;
            rTrans.offsetMin = Vector2.zero;
            rTrans.offsetMax = Vector2.zero;
        }

        // Animation
        public static void Play(this Animation anim, AnimationClip clip){
            anim.Rewind();
            anim.clip = clip;
            anim.Play();
        }

        

    public static Vector3 XZ3(this Vector2 xy){
        return new Vector3(xy.x, 0f, xy.y);
    }

    public static Vector2 XY(this Vector3 xyz){
        return new Vector2(xyz.x, xyz.y);
    }

    public static Quaternion CalcNoRoll(this Quaternion rot){
        Vector3 euler = rot.eulerAngles;
        euler.z = 0f;
        return Quaternion.Euler(euler);
    }

    private static Vector3[] s_fourCorners = new Vector3[] {
        Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero
    };

    // NOTE: this returns items in the range of rootCanvas's rectTransform's sizeDelta
    public static void GetScreenCorners(this RectTransform rt, 
        out Vector2 bottomLeft, out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomRight 
    ) {
        rt.GetWorldCorners(s_fourCorners);

        Canvas rootCanvas = rt.GetComponentInParent<Canvas>().rootCanvas;
        Camera refCam = rootCanvas.worldCamera ?? Camera.main;
        
        for(int i = 0; i < 4; i++){
            s_fourCorners[i] = refCam.WorldToScreenPoint(s_fourCorners[i]);
        }

        bottomLeft = s_fourCorners[0].XY();
        topLeft = s_fourCorners[1].XY();
        topRight = s_fourCorners[2].XY();
        bottomRight = s_fourCorners[3].XY();
    }

    // NOTE: this returns items in the range of rootCanvas's rectTransform's sizeDelta
    public static Rect GetScreenRect(this RectTransform rectTransform){
        Vector2 bl, br, tl, tr;
        GetScreenCorners(rectTransform, out bl, out tl, out tr, out br);
        return new Rect(bl, tr-bl);
    }

    // TODO: NEXT TIME YOU'RE HERE
    // MOVE ALL THIS TO UIUtils

    // Bottom-left is 0,0; top-right is 1,1.
    public static Rect GetNormalizedScreenRect(this RectTransform rectTransform){
        Rect screenRect = GetScreenRect(rectTransform);
        Canvas c = rectTransform.GetComponentInParent<Canvas>().rootCanvas;
        c.TryGetComponent<RectTransform>(out var rt);
        var dims = rt.rect.size;
        return new Rect(screenRect.position / dims, screenRect.size / dims);
    }

    public static Vector3 WorldToScreenPoint(this Camera camera, Vector3 worldPosition, CanvasScaler canvasScaler){
        // Good / bad / good news:
        // Good: this fixes the ui render positions
        // bad: this screws up the ui raycaster positioning
        // Good: you only should use splitscreen for consoles, where ui raycasting isn't a thing!

        // Rect oldViewportRect = camera.rect;
        // camera.rect = new Rect(0,0, 1f, 1f);
		Vector3 screenPos = camera.WorldToScreenPoint (worldPosition);
        // camera.rect = oldViewportRect;

		return canvasScaler.Scale (new Vector2 (screenPos.x, screenPos.y));
	}

    public static Vector2 Screen2AnchorPoint(this Vector2 screenPoint, Vector2 childElementMinAnchor, RectTransform fullscreenParent, CanvasScaler scaler){
        Vector2 parentSize = fullscreenParent.rect.size;
        Vector2 pixelAnchor = Vector2.Scale(childElementMinAnchor, parentSize);
        Vector2 anchorPoint = screenPoint - pixelAnchor;

        return anchorPoint;
    }

    public static Vector2 WorldToAnchorPoint(this Camera camera, Vector3 worldPosition, RectTransform childOfFullscreenParent, CanvasScaler scaler){
        Vector2 fromBotLeft = camera.WorldToScreenPoint(worldPosition, scaler);
        Vector2 myMinAnchor = childOfFullscreenParent.anchorMin;

        return fromBotLeft.Screen2AnchorPoint(myMinAnchor, (RectTransform)(childOfFullscreenParent.parent), scaler);
    }
    // 6/12/2020. What kicked my ass: use rect.size over size delta, especially when dealing with wide anchors

    public static Vector2 GetNormalizedPositionWithinParent(this RectTransform uiElement){
        Vector2 parentSize = ((RectTransform)(uiElement.parent)).rect.size;
        Vector2 myMinAnchor = uiElement.anchorMin;
        Vector2 pxAnchor = Vector2.Scale(myMinAnchor, parentSize);

        Vector2 fromBotLeft = uiElement.anchoredPosition + pxAnchor;
        Vector2 fromBotLeftNorm = Vector2.Scale(fromBotLeft, parentSize);
        return fromBotLeftNorm;
    }

	public static float GetFieldofViewHorizontal(this Camera camera){
		float vertFOV = camera.fieldOfView;
		float radVert = vertFOV * Mathf.Deg2Rad;
		float radHorz = 2f * Mathf.Atan(Mathf.Tan(radVert / 2) * camera.aspect);
		float horzFOV = radHorz * Mathf.Rad2Deg;
		return horzFOV;
	}

	public static Vector2 Scale(this CanvasScaler canvasScaler, Vector2 screenPosition){
        switch(canvasScaler.uiScaleMode){
        default:
        case CanvasScaler.ScaleMode.ScaleWithScreenSize: {
            float xFactor = canvasScaler.referenceResolution.x / Screen.width;
            float yFactor = canvasScaler.referenceResolution.y / Screen.height;
            float scaleFactor = Mathf.Lerp (xFactor, yFactor, canvasScaler.matchWidthOrHeight);
            return screenPosition * scaleFactor;
        }
        case CanvasScaler.ScaleMode.ConstantPixelSize: {
            return screenPosition; // * canvasScaler.scaleFactor;
        }
        }
	}

	public static Vector2 Unscale(this CanvasScaler scaler, Vector2 worldPosition){
		float xFactor =  Screen.width / scaler.referenceResolution.x;
		float yFactor =  Screen.height / scaler.referenceResolution.y;
		float scaleFactor = Mathf.Lerp (xFactor, yFactor, scaler.matchWidthOrHeight);
		return worldPosition * scaleFactor;
	}

    // Event System Stuff
    public static void ChangeSelection(this EventSystem es, GameObject newSelection){
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(newSelection);
    }

    public static void ChangeSelection(this EventSystem es, GameObject newSelection, BaseEventData pointer){
        es.SetSelectedGameObject(null, pointer);
        es.SetSelectedGameObject(newSelection, pointer);
    }

    // AudioCilp
    public static double DSPLength(this AudioClip clip){
        double songLength = ((double)clip.samples) / ((double)clip.frequency);
        return songLength;
    }

    // Box Collider
    public static Vector3 GetBoxPoint(this BoxCollider bc, Vector3 signedNormXYZ){
        Vector3 lossyScale = bc.transform.lossyScale;
        Quaternion worldRotation = bc.transform.rotation;
        Vector3 localSpaceOffset = bc.center;
        Vector3 localBoxSize = bc.size;

        Vector3 trueBoxCenter = bc.transform.position + worldRotation*Vector3.Scale(localSpaceOffset, lossyScale);
        Vector3 worldOffset = Vector3.Scale( localBoxSize*0.5f, Vector3.Scale(lossyScale, (worldRotation * signedNormXYZ)) );
        return worldOffset;
    }

    public static void GetCapsuleHemisphereCenters(this CapsuleCollider cc, out Vector3 a, out Vector3 b){
        Vector3 lossyScale = cc.transform.lossyScale;
        Quaternion worldRotation = cc.transform.rotation;
        Vector3 localSpaceOffset = cc.center;
        
        Vector3 trueCapsuleCenter = cc.transform.position + worldRotation*Vector3.Scale( localSpaceOffset, lossyScale );
        float trueFullHeight = cc.height * lossyScale[cc.direction];
        float length2HemisphereCenter = (trueFullHeight - 2f*(cc.radius * Mathf.Max(lossyScale.x,Mathf.Max(lossyScale.y, lossyScale.z)) )) / 2f;
        Vector3 localHeightDir;
        switch(cc.direction){
        default:
        case 0: localHeightDir = Vector3.right; break;
        case 1: localHeightDir = Vector3.up; break;
        case 2: localHeightDir = Vector3.forward; break;
        }
        Vector3 aOffset = length2HemisphereCenter * (worldRotation*localHeightDir);
        a = trueCapsuleCenter + aOffset;
        b = trueCapsuleCenter - aOffset;
    }

    }   

}