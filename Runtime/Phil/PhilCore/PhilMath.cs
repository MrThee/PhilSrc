using UnityEngine;
using System;
using System.Collections.Generic;

namespace Phil {

public static class Math {

	public const float Phi = 1.61803398875f;

	public static readonly System.Func<float, float, float> AddFloats = (a,b) => a+b;
	public static readonly System.Func<Vector2, Vector2, Vector2> AddVector2 = (a,b) => a+b;
	public static readonly System.Func<Vector3, Vector3, Vector3> AddVector3 = (a,b) => a+b;
	public static readonly System.Func<Vector4, Vector4, Vector4> AddVector4 = (a,b) => a+b;
	public static readonly System.Func<float, int, float> DivideFloat = (a,b) => a/b;
	public static readonly System.Func<Vector2, int, Vector2> DivideVector2 = (a,b) => a/b;
	public static readonly System.Func<Vector3, int, Vector3> DivideVector3 = (a,b) => a/b;
	public static readonly System.Func<Vector4, int, Vector4> DivideVector4 = (a,b) => a/b;

	public static float CalcAverage(IReadOnlyList<float> ql){ return CalcAverage(ql, AddFloats, DivideFloat); }
	public static Vector2 CalcAverage(IReadOnlyList<Vector2> ql){ return CalcAverage(ql, AddVector2, DivideVector2); }
	public static Vector3 CalcAverage(IReadOnlyList<Vector3> ql){ return CalcAverage(ql, AddVector3, DivideVector3); }
	public static Vector4 CalcAverage(IReadOnlyList<Vector4> ql){ return CalcAverage(ql, AddVector4, DivideVector4); }

	public static T CalcAverage<T>(IReadOnlyList<T> quantityList, System.Func<T,T,T> AddFunc, System.Func<T,int,T> IntDivide){
		int qc = quantityList.Count;
		T sum = default(T);
		for(int i = 0; i < qc; i++){
			sum = AddFunc(sum, quantityList[i]);
		}
		return IntDivide(sum, qc);
	}

	public static int FloorToIndex(float norm_t, int elementCount){
		if(norm_t >= 1f){
			return elementCount-1;
		}
		return Mathf.FloorToInt( norm_t / (1f/elementCount) );
	}

	public static float FloorToGranularity(float value, float granularity){
		return Mathf.FloorToInt(value / granularity) * granularity;
	}

	public static float RoundToGranularity(float value, float granularity){
		return Mathf.RoundToInt(value / granularity) * granularity;
	}

	public static Vector2 RoundToGranularity(Vector2 value, float granularity){
		return new Vector2( RoundToGranularity(value.x, granularity), RoundToGranularity(value.y, granularity) );
	}

	public static float Remap(float value, float oldA, float oldB, float newA, float newB){
		return Mathf.Lerp( newA, newB, Mathf.InverseLerp(oldA, oldB, value) );
	}

	public static bool WithinRange(float lowerBound, float midValue, float upperBound){
		return (lowerBound < midValue && midValue <= upperBound);
	}

	public static int SawtoothMod(int x, int m){
		if(x >= 0){
			return x%m;
		} else {
			int pos = -x;
			int modded = pos%m;
			return (m - modded)%m;
		}
	}

	public static Vector3 ConicallyDeviateRandomly(Vector3 baseVector, float maxThetaAngleDeviation){
		Vector3 firstCrossOperand = (Mathf.Abs(Vector3.Dot(baseVector, Vector3.up)) < 0.95f) ? Vector3.up : Vector3.forward;
		Vector3 rotatorRotatee = Vector3.Cross(baseVector, firstCrossOperand);
		float randPhiAngle = UnityEngine.Random.Range(0f, 360f);
		rotatorRotatee = Quaternion.AngleAxis(randPhiAngle, baseVector.normalized) * rotatorRotatee;
		float randThetaAngle = UnityEngine.Random.Range(0f, maxThetaAngleDeviation);
		Vector3 finalVector = Quaternion.AngleAxis(randThetaAngle, rotatorRotatee) * baseVector;
		return finalVector;
	}

}}