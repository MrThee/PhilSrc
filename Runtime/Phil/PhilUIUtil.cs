using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Phil {

public static class UIUtils {

	public static void PrePoolGraphicGeo(Graphic graphic){
		graphic.SetVerticesDirty();
		graphic.SetMaterialDirty();
		graphic.Rebuild( CanvasUpdate.PreRender );
		// graphic.Rebuild( CanvasUpdate.Layout );
	}

	public static void PrePoolTMProTextField(TMPro.TMP_Text textField){
		textField.ForceMeshUpdate();
		PrePoolGraphicGeo(textField);
	}

    public static void SelectAndConfirm(GameObject gameObject){
		EventSystem eventSystem = EventSystem.current;
		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(gameObject);
		ExecuteEvents.Execute (gameObject, new BaseEventData (eventSystem), ExecuteEvents.submitHandler);
	}

	public static void BindNavPlus(Selectable center, Selectable up, Selectable down,
		Selectable left, Selectable right)
	{
		Navigation nav = center.navigation;
		nav.selectOnDown = down;
		nav.selectOnUp = up;
		nav.selectOnLeft = left;
		nav.selectOnRight = right;
		center.navigation = nav;
	}

	public static void BindNavHorizontal(Selectable left, Selectable right){
		Navigation leftNav = left.navigation;
		Navigation rightNav = right.navigation;
		leftNav.selectOnRight = right;
		rightNav.selectOnLeft = left;
		left.navigation = leftNav;
		right.navigation = rightNav;
	}

	public static void BindNavVertical(Selectable top, Selectable bottom){
		Navigation topNav = top.navigation;
		Navigation botNav = bottom.navigation;
		topNav.selectOnDown = bottom;
		botNav.selectOnUp = top;
		top.navigation = topNav;
		bottom.navigation = botNav;
	}

	public static void BindAsGridNavigation(List<Selectable> selectables, int gridWidth){
		Selectable current;
		Selectable up, down, left, right = null;
		for (int i = 0; i < selectables.Count; i++) {
			current = selectables [i];
			int iDown = i + gridWidth;
			int iUp = i - gridWidth;
			int iRight = i + 1;
			int iLeft = i - 1;
			// Clamping + wraping...?
//			if (iDown >= selectables.Count) {
//				// Do something?
//			}
//			if (iUp < 0) {
//				int blockSize = gridWidth * ((selectables.Count / gridWidth));
//				// Need to figure out the most intelligent way to handle this...
//			}
			// Leave iRight and iLeft alone...
			if (iLeft < 0) {
				iLeft += selectables.Count;
			}
			if (iRight >= selectables.Count) {
				iRight = 0;
			}
			current = selectables[i];
			right = selectables[iRight];
			left = selectables[iLeft];
			up = (iUp >= 0) ? selectables [iUp] : current;
			down = (iDown < selectables.Count) ? selectables [iDown] : current;
			BindNavPlus (current, up, down, left, right);
		}
	}

	public static void AlignToLattice(List<RectTransform> rects, Vector2 origin, int minorAxisSlotCount,
		Vector2 minorAxisDelta, Vector2 majorAxisDelta)
	{
		for (int i = 0; i < rects.Count; i++) {
			int x = i % minorAxisSlotCount;
			int y = i / minorAxisSlotCount;
			Vector2 offset = x * minorAxisDelta + y * majorAxisDelta;
			Vector2 anchoredPosition = origin + offset;

			rects [i].anchoredPosition = anchoredPosition;
		}
	}

	public static void AlignToLattice<T>(List<T> hasRects, System.Func<T, RectTransform> rectFetcher, 
		Vector2 origin, int minorAxisSlotCount, Vector2 minorAxisDelta, Vector2 majorAxisDelta)
	{
		for (int i = 0; i < hasRects.Count; i++) {
			int x = i % minorAxisSlotCount;
			int y = i / minorAxisSlotCount;
			Vector2 offset = x * minorAxisDelta + y * majorAxisDelta;
			Vector2 anchoredPosition = origin + offset;

			rectFetcher(hasRects[i]).anchoredPosition = anchoredPosition;
		}
	}

	// Note: the 2 axes specified don't have to be orthogonal to each other.
	// You can layout things like parallelograms this way.
	// Note 2: this assumes the element you're aligning has min/max anchors of (0.5, 0.5).
	public static void CalcCenteredLatticeParams(RectTransform latticeContainer, Vector2 borderPadding,
		Vector2 minorAxisDir, Vector2 majorAxisDir, int minorCount, int majorCount,
		out Vector2 origin, out Vector2 minorAxisDelta, out Vector2 majorAxisDelta) 
	{
		Vector2 containerSize = latticeContainer.rect.size;
		
		// Vector2 areaSize = containerSize - 2f*borderPadding;
	// 	float minorDotAreaLength = Mathf.Abs(Vector2.Dot(minorAxisDir, areaSize));
	// 	float majorDotAreaLength = Mathf.Abs(Vector2.Dot(majorAxisDir, areaSize));

	// 	minorAxisDelta = minorCount > 1 ? minorAxisDir * minorDotAreaLength/ (minorCount-1) : Vector2.zero;
	// 	majorAxisDelta = majorCount > 1 ? majorAxisDir * majorDotAreaLength/ (majorCount-1) : Vector2.zero;

	// 	Vector2 recenterOffset = -0.5f*(minorDotAreaLength * minorAxisDir + majorDotAreaLength * majorAxisDir);

	// 	Vector2 borderCornerOffset = (borderPadding.x * minorAxisDir + borderPadding.y * majorAxisDir);
	// 	origin = recenterOffset; // + borderCornerOffset + minorAxisDelta + majorAxisDelta;

		// TODO: this better.
		// Compare 
			// minorAxisDir.Normalize();
			// majorAxisDir.Normalize(); 

			Vector2 halfContainerSize = 0.5f * containerSize;
			
			// minor/major axis dir form a rhombus.
			// 1. Calculate the AABB of this rhombus(?)
			Vector2 rhombStart = Vector2.zero;
			Vector2 rhombEnd = rhombStart + minorAxisDir + majorAxisDir;
			Vector2 min = Vector2.Min(rhombStart, rhombEnd);
			Vector2 max = Vector2.Max(rhombStart, rhombEnd);
			Rect rhombAABB = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
			Vector2 rhombSize = rhombAABB.size;
			// Normalize it.
			rhombSize = (rhombSize.x > rhombSize.y) ? rhombSize / rhombSize.x : rhombSize/rhombSize.y;
			rhombAABB.size = rhombSize;

			Vector2 insetSize = containerSize - 2*borderPadding;
			Vector2 rhombVecScaler = new Vector2(insetSize.x / rhombAABB.width, insetSize.y / rhombAABB.height);

			Rect centeredLatticeArea = new Rect(-insetSize/2, insetSize);
			float normOrigX = CalcNormOrig(minorAxisDir.x*minorCount, majorAxisDir.x*majorCount);
			float normOrigY = CalcNormOrig(minorAxisDir.y*minorCount, majorAxisDir.y*majorCount);
			float xOrigin = Mathf.Lerp(centeredLatticeArea.xMin, centeredLatticeArea.xMax, normOrigX);
			float yOrigin = Mathf.Lerp(centeredLatticeArea.yMin, centeredLatticeArea.yMax, normOrigY);
			origin = new Vector2(xOrigin, yOrigin);

			Vector2 minorScaled = rhombVecScaler.x * minorAxisDir;
			Vector2 majorScaled = rhombVecScaler.y * majorAxisDir;
			minorAxisDelta = minorScaled / (minorCount - 1);
			majorAxisDelta = majorScaled / (majorCount - 1);
	}

	public static Vector2 CalcLocalInsetPosition(RectTransform container, RectTransform.Edge edge, float inset){
		Vector2 halfSize = container.rect.size / 2;
		switch(edge){
			case RectTransform.Edge.Top:
			return (halfSize.y - inset) * Vector2.up;

			case RectTransform.Edge.Bottom:
			return (halfSize.y - inset) * Vector2.down;

			default:
			case RectTransform.Edge.Left:
			return (halfSize.x - inset) * Vector2.left;

			case RectTransform.Edge.Right:
			return (halfSize.x - inset) * Vector2.right;
		}
	}

	public static Vector2 CalcCenteredOffsetOrigin(Vector2 elementStride, int elementCount)
	{
		Vector2 halfStride = elementStride / 2;
		Vector2 offset = -halfStride*(elementCount-1);
		return offset;
	}

	static float CalcNormOrig(float a, float b){
		if(a >= 0f && b >= 0f){
			return 0f;
		}
		else if(a <= 0f && b <= 0f){
			return 1f;
		}
		else {
			float start = Mathf.Min(a,b);
			float end = Mathf.Max(a,b);
			return Mathf.InverseLerp(start, end, 0f);
		}
	}

	public static T Instantiate<T>(T pfUI, RectTransform parent) where T:Object{
		return Object.Instantiate(pfUI, parent, false);
	}

	public static T Instantiate<T>(T pfUI, RectTransform parent, 
		System.Func<T, RectTransform> RectFetcher, 
		System.Action<RectTransform> RectFormatter
	) where T:Object {
		T newUIElement = Object.Instantiate(pfUI, parent, false);
		RectFormatter(RectFetcher(newUIElement));
		return newUIElement;
	}

	public static void AnchorsToCorners(RectTransform t){
		RectTransform pt = t.parent as RectTransform;

		if(t == null || pt == null) return;
		
		Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
											t.anchorMin.y + t.offsetMin.y / pt.rect.height);
		Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
											t.anchorMax.y + t.offsetMax.y / pt.rect.height);

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2(0, 0);
	}

	public static void CornersToAnchors(RectTransform t){
		t.offsetMin = t.offsetMax = new Vector2(0, 0);
	}

}

}