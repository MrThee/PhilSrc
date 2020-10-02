using UnityEngine;

namespace Phil {

public static class Math {

	public const float Phi = 1.61803398875f;

	public static int FloorToIndex(float norm_t, int elementCount){
		if(norm_t >= 1f){
			return elementCount-1;
		}
		return Mathf.FloorToInt( norm_t / (1f/elementCount) );
	}

	public static float Remap(float value, float oldA, float oldB, float newA, float newB){
		return Mathf.Lerp( newA, newB, Mathf.InverseLerp(oldA, oldB, value) );
	}

	public static Vector3 ConicallyDeviateRandomly(Vector3 baseVector, float maxThetaAngleDeviation){
		Vector3 firstCrossOperand = (Mathf.Abs(Vector3.Dot(baseVector, Vector3.up)) < 0.95f) ? Vector3.up : Vector3.forward;
		Vector3 rotatorRotatee = Vector3.Cross(baseVector, firstCrossOperand);
		float randPhiAngle = Random.Range(0f, 360f);
		rotatorRotatee = Quaternion.AngleAxis(randPhiAngle, baseVector.normalized) * rotatorRotatee;
		float randThetaAngle = Random.Range(0f, maxThetaAngleDeviation);
		Vector3 finalVector = Quaternion.AngleAxis(randThetaAngle, rotatorRotatee) * baseVector;
		return finalVector;
	}

}}