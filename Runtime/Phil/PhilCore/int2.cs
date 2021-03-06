using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Phil.Core {

[System.Serializable]
public struct int2 : IEquatable<int2> {

	public int x;
	public int y;

	public int2(int x, int y){
		this.x = x;
		this.y = y;
	}
	public int2(Vector3 position){
		this.x = Mathf.RoundToInt (position.x);
		this.y = Mathf.RoundToInt (position.z);
	}

	public int ManhattanDist(int2 destination){
		int2 diff = this - destination;
		return Mathf.Abs (diff.x) + Mathf.Abs (diff.y);
	}

	public Vector2 vec2 { get { return new Vector2 (this.x, this.y); } }
	public Vector3 worldPos { get { return new Vector3 (this.x, 0f, this.y); } }
	public int area { get { return Mathf.Abs (this.x) * Mathf.Abs (this.y); } }
	public int2 normalized { get { return new int2 (Mathf.Clamp (this.x, -1, 1), Mathf.Clamp (this.y, -1, 1)); } }

	public static int2 operator +(int2 a, int2 b){
		return new int2( a.x+b.x, a.y+b.y );
	}
	public static int2 operator -(int2 a, int2 b){
		return new int2( a.x-b.x, a.y-b.y );
	}
	public static int2 operator *(int2 a, int f){
		return new int2 (a.x * f, a.y * f);
	}
	public static int2 operator *(int f, int2 a){
		return new int2 (a.x * f, a.y * f);
	}
	public static int2 operator /(int2 a, int d){
		return new int2 (a.x / d, a.y / d);
	}
	public static int2 operator /(int2 a, int2 b){
		return new int2(a.x/b.x, a.y/b.y);
	}
	public static int2 operator *(int2 a, int2 b){
		return new int2 (a.x * b.x, a.y * b.y);
	}
	public static bool operator ==(int2 a, int2 b ){
		return (a.x == b.x && a.y == b.y );
	}
	public static bool operator !=(int2 a, int2 b){
		return (a.x != b.x || a.y != b.y);
	}

	public bool Equals(int2 other){
		return this == other;
	}

	// Handy static properties
	public static int2 zero	{ get { return new int2 (0, 0); } }
	public static int2 up	{ get { return new int2 (0, 1); } }
	public static int2 down	{ get { return new int2 (0,-1); } }
	public static int2 right{ get { return new int2 (1, 0); } }
	public static int2 left	{ get { return new int2 (-1,0); } }
	public static int2 one  { get { return new int2 (1, 1); } }

	public static int2 Random(int2 minInclusive, int2 maxExclusive){

		if (minInclusive.x >= maxExclusive.x || minInclusive.y >= maxExclusive.y) {
			Debug.LogError ("int2 minInclusive is greater than or equal to int2MaxExclusive");
			return int2.zero;
		}

		int randX = UnityEngine.Random.Range (minInclusive.x, maxExclusive.x);
		int randY = UnityEngine.Random.Range (minInclusive.y, maxExclusive.y);
		return new int2 (randX, randY);
	}

	public int2 north { get { return this + int2.up; } }
	public int2 south { get { return this + int2.down; } }
	public int2 west { get { return this + int2.left; } }
	public int2 east { get { return this + int2.right; } }

	public static int2 Min(int2 a, int2 b){
		return new int2 (Mathf.Min (a.x, b.x), Mathf.Min (a.y, b.y));
	}

	public static int2 Max(int2 a, int2 b){
		return new int2 (Mathf.Max (a.x, b.x), Mathf.Max (a.y, b.y));
	}

	public static int2 Clamp(int2 value, int2 min, int2 max){
		return new int2(
			Mathf.Clamp(value.x, min.x, max.x),
			Mathf.Clamp(value.y, min.y, max.y)
		);
	}

    public int GetSpiralFlatIndex(){
        int ringNumber = Mathf.Max( Mathf.Abs(this.x), Mathf.Abs(this.y) );
        int twoRN = 2*ringNumber;
        int squareSide = (twoRN) + 1;
        
        int quant = (2*(ringNumber-1)) + 1;
        int northIndex = quant * quant;
        int northEastIndex = northIndex + ringNumber;
        int southEastIndex = northEastIndex + twoRN;
        int southWestIndex = southEastIndex + twoRN;
        int northWestIndex = southWestIndex + twoRN;

        if(this.x == ringNumber){
            // East wall
            int2 northEast = int2.zero.SpiralOut(northEastIndex);
            int downwards = Mathf.Abs( this.y - northEast.y );
            return northEastIndex + downwards;
        } else if(this.x == -ringNumber){
            // West wall
            int2 southWest = int2.zero.SpiralOut(southWestIndex);
            int upwards = this.y - southWest.y;
            return southWestIndex + upwards;
        } else if(this.y == ringNumber){
            // North wall
            int2 north = int2.zero.SpiralOut(northIndex);
            int2 northWest = int2.zero.SpiralOut(northWestIndex);
            return (this.x >= north.x) ? (this.x - north.x) + northIndex : (this.x - northWest.x) + northWestIndex;
        } else { // this.y == -ringNumber
            // South wall
            int2 southEast = int2.zero.SpiralOut(southEastIndex);
            int westward = Mathf.Abs( this.x - southEast.x );
            return southEastIndex + westward;
        }
    }

	public int2 SpiralOut(int flatIndex){
		if (flatIndex <= 0)
			return this;

		switch (flatIndex) {
		case 1:
			return this + int2.up;
		case 2:
			return this + int2.one;
		case 3:
			return this + int2.right;
		case 4:
			return this + new int2 (1, -1);
		case 5:
			return this + int2.down;
		case 6:
			return this - int2.one;
		case 7:
			return this + int2.left;
		case 8:
			return this + new int2 (-1, 1);
		default:
			break;
		}

		int lowerSqrt = Mathf.FloorToInt (Mathf.Sqrt ((float)flatIndex));
		int oddLowerSqrt = (lowerSqrt % 2 == 0) ? lowerSqrt - 1 : lowerSqrt;

		int distFromCenter = (oddLowerSqrt/2) + 1; // 1->1; 3->2; 5->3; 7->4; ...

		int minValueOnRing = oddLowerSqrt*oddLowerSqrt; // N_post 
		int maxValueOnRing = (oddLowerSqrt+2) * (oddLowerSqrt+2) -1; // West of N_post
		int ringSideLength = 2 * distFromCenter + 1;

		int NE_post = minValueOnRing + distFromCenter;
		int SE_post = NE_post + ringSideLength - 1;
		int SW_post = SE_post + ringSideLength - 1;
		int NW_post = SW_post + ringSideLength - 1;

		if(flatIndex == minValueOnRing)
			return this + int2.up * distFromCenter;
		if(flatIndex == maxValueOnRing)
			return this + int2.up * distFromCenter - int2.left;
		if(flatIndex == NE_post)
			return this + int2.one * distFromCenter;
		if(flatIndex == SE_post)
			return this + new int2 (1, -1) * distFromCenter;
		if (flatIndex == SW_post)
			return this - int2.one * distFromCenter;
		if (flatIndex == NW_post)
			return this + new int2 (-1, 1) * distFromCenter;


		// Somewhere in between....
		if (flatIndex > NE_post && flatIndex < SE_post) {
			// East edge.
			int2 NE =  this + int2.one * distFromCenter;
			int diff = flatIndex - NE_post;
			return NE + int2.down * diff;
		}
		if (flatIndex > SE_post && flatIndex < SW_post) {
			// South edge
			int2 SE = this + new int2 (1, -1) * distFromCenter;
			int diff = flatIndex - SE_post;
			return SE + int2.left * diff;
		}
		if (flatIndex > SW_post && flatIndex < NW_post) {
			// West edge
			int2 SW = this - int2.one * distFromCenter;
			int diff = flatIndex - SW_post;
			return SW + int2.up * diff;
		}
		if (flatIndex > NW_post && flatIndex < maxValueOnRing) {
			// North edge; west half
			int2 NW = this + new int2 (-1, 1) * distFromCenter;
			int diff = flatIndex - NW_post;
			return NW + int2.right * diff;
		}
		if (flatIndex > minValueOnRing && flatIndex < NE_post) {
			// North edge; east half
			int2 N = this + int2.up * distFromCenter;
			int diff = flatIndex - minValueOnRing;
			return N + int2.right * diff;
		}
			
		Debug.LogError("You fucked up. Flat index: " + flatIndex);
		return this;
	}

	public override string ToString ()
	{
		return string.Format ("[int2: x={0}, y={1}]", x, y);
	}

	public override int GetHashCode(){
		return x ^ y;
	}

	public override bool Equals(object obj){
		throw new System.ArgumentException("NO STRUCT BOXING!!!");
	}
}

}
