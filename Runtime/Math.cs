using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonloop.Core
{
	public static class Math
	{
		/// <summary>
		/// Returns the positive position of the y coordinate of a circle given the circle's radius and x position.
		/// </summary>
		/// <param name="radius">Circle's radius in real units</param>
		/// <param name="x">The x position on the circle, where 0 is the origin, and 1 is the farthest right on the circle</param>
		public static float PositionYOfCircle(float radius, float x)
		{
			return Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(radius * x, 2));
		}

		/// <summary>
		/// Takes a world space direction and returns the horizontal angle of it. For example, tells if the direction
		/// is facing north, east, west, etc. But uses euler angle drgrees 
		/// </summary>
		public static float AngleFromWorldDirection(Vector3 direction) 
		{
			var projectedDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
			projectedDirection.Normalize();
			return Mathf.Atan2(projectedDirection.z, projectedDirection.x) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Returns the euler angle of the given vector.
		/// </summary>
		/// <param name="degreesOffset">Degrees to offset the result by</param>
		public static float AngleFromVector2(Vector2 vector, float degreesOffset)
		{
			Vector2 rotatedVector = Quaternion.Euler(0, 0, degreesOffset) * vector;
			float rad = Mathf.Atan2(rotatedVector.y, rotatedVector.x);
			return Mathf.Rad2Deg * rad;
		}
		
		public static Vector2 RadiansToVector2(float radian)
		{
			return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
		}
  
		public static Vector2 DegreeToVector2(float degree)
		{
			return RadiansToVector2(degree * Mathf.Deg2Rad);
		}

		/// <summary>
		/// Given the input, returns a number that's rounded to the nearest increment. For example, given an input
		/// of 7.55 and an increment of 5, will round to 10. An input of 6 with increment 5 will round to 5. Only accepts
		/// positive rounding increments.
		/// </summary>
		public static float RoundToNearest(float input, float roundingIncrement)
		{
			roundingIncrement = Mathf.Abs(roundingIncrement);
			
			// failsafe for rounding increments near 0
			if (roundingIncrement <= Mathf.Epsilon) return input;
			
			float remainder = Mathf.Abs(input) % roundingIncrement;
			float half = roundingIncrement / 2;

			if (input >= 0)
			{
				if (remainder < half)
					return input - remainder;
				else
					return input + roundingIncrement - remainder;
			}

			// negative numbers are fun
			if (remainder < half)
				return input + remainder;
			else
				return input - roundingIncrement + remainder;
			
		}

		/// <summary> Returns the largest absolute value of each component (x, y, z) </summary>
		public static Vector3 MaxCombine(Vector3 lhs, Vector3 rhs) 
		{
			return new Vector3(
				MaxCombine(lhs.x, rhs.x),
				MaxCombine(lhs.y, rhs.y),
				MaxCombine(lhs.z, rhs.z)
			);
		}

		static float MaxCombine(float value1, float value2) 
		{
			if (Mathf.Abs(value1) > Mathf.Abs(value2))
				return value1;
			else
				return value2;
		}

		/// <summary>
		/// Given a vector of a direction in 2D space (x, y) returns a vector that's the 2D
		/// vector projected on to the ground. Useful for taking a Vector2 input (like gamepad joystick)
		/// and returning a vector that will move an object along the ground.
		/// </summary>
		public static Vector3 Project2Dto3D(Vector2 flatVector) => new Vector3(flatVector.x, 0, flatVector.y);

		/// <summary>
		/// Given a vector of a 3d direction in space, returns a vector that's 2D (x, y)
		/// </summary>
		public static Vector2 Project3Dto2D(Vector3 vector) => new Vector2(vector.x, vector.z);

		/// <summary>
		/// Returns a random element of the given list.
		/// </summary>
		public static T RandomElementOfList<T>(List<T> list)
		{
			int selectedIndex = Random.Range(0, list.Count);
			return list[selectedIndex];
		}

		public static bool LayerMaskContainsLayer(LayerMask layerMask, int layer)
		{
			return layerMask == (layerMask | (1 << layer));
		}

		/// <summary>
		/// Given any arbitrary angle, returns that same angle but expressed as a number between 0 and 360
		/// </summary>
		public static float Angle0to360(float inputAngle)
		{
			if (inputAngle >= 0) return inputAngle % 360;

			inputAngle = Mathf.Abs(inputAngle);
			float remainder = inputAngle % 360;
			return 360 - remainder;
		}

		public static float AngleMinus180to180(float inputAngle)
		{
			inputAngle = Angle0to360(inputAngle);
			if (inputAngle > 180) {
				float diff = inputAngle - 180;
				return -180 + diff;
			}
			return inputAngle;
		}

		public static Vector3 Parabola(Vector3 start, float height, Vector3 end, float progress) 
		{
			Vector3 result = Vector3.Lerp(start, end, progress);

  			// draw a quadratic curve using progress as the input
			result.y += (-4 * height * progress * (progress - 1));

			return result;
		}

		/// <summary> Returns the nearest point on or in the given circle to the given test point. Assumes an X,Z circle on the Y plane </summary>
		public static Vector3 NearestPointOnCircle(Vector3 testPoint, Vector3 circleCenter, float radius)
		{
			Vector3 direction = (testPoint - circleCenter);
			direction.y = 0;

			if (direction.sqrMagnitude < radius * radius) 
				return testPoint;

			direction.Normalize();
			direction *= radius;
			return circleCenter + direction;
		}


		public static string ColorToHex(Color color) => $"{color.r:X2}{color.g:X2}{color.b:X2}";

		/// <summary> Returns true if the vector's magnitude is within the given max range. Uses efficient square-magnitude calculation</summary>
		public static bool IsInRange(Vector3 vector, float maxRange) => vector.sqrMagnitude <= (maxRange * maxRange);

		/// <summary> Returns true if the vector's magnitude is outside the given max range. Uses efficient square-magnitude calculation</summary>
		public static bool OutOfRange(Vector3 vector, float maxRange) => !IsInRange(vector, maxRange);

		public static Vector3 Center(List<Vector3> points) 
		{
			Vector3 sum = Vector3.zero;
			for (int i = 0; i < points.Count; i++) {
				sum += points[i];
			}

			return sum / points.Count;
		}

		/// <summary>
		/// Checks if the difference between 2 numbers is less than Epsilon. 
		/// Basically a way to eliminate noisy results when comparing 2 very close floats
		/// </summary>
		public static bool IsSameNumber(float numberA, float numberB) => Mathf.Abs(numberA - numberB) < Mathf.Epsilon;

		/// <summary>
		/// Returns a point at time t for a bezier curve.
		/// </summary>
		/// <param name="t">time of the curve between 0 and 1</param>
		/// <param name="startPoint"></param>
		/// <param name="startAnchor"></param>
		/// <param name="endAnchor"></param>
		/// <param name="endPoint"></param>
		/// <returns></returns>
		public static Vector3 GetBezier(float t, Vector3 startPoint, Vector3 startAnchor, Vector3 endAnchor, Vector3 endPoint)
		{
			Vector3 c = 3 * (startAnchor - startPoint);
			Vector3 b = 3 * (endAnchor - startAnchor) - c;
			Vector3 a = endPoint - startPoint - c - b;

			float Cube = t * t * t;
			float Square = t * t;

			return (a * Cube) + (b * Square) + (c * t) + startPoint;
		}

		/// <summary>
		/// Returns the tangent of the bezier curve at time t.
		/// </summary>
		/// <param name="t">Time of the curve between 0 and 1</param>
		/// <param name="startPoint"></param>
		/// <param name="startAnchor"></param>
		/// <param name="endAnchor"></param>
		/// <param name="endPoint"></param>
		/// <returns></returns>
		public static Vector3 GetBezierTangent(float t, Vector3 startPoint, Vector3 startAnchor, Vector3 endAnchor, Vector3 endPoint) 
		{
			t = Mathf.Clamp01(t);
			float p1 = Mathf.Clamp01(t - 0.01f);
			float p2 = Mathf.Clamp01(t + 0.01f);

			Vector3 point1 = GetBezier(p1, startPoint, startAnchor, endAnchor, endPoint);
			Vector3 point2 = GetBezier(p2, startPoint, startAnchor, endAnchor, endPoint);

			return (point2 - point1).normalized;
		}

		public static Vector3 GetBezierNormal(float t, Vector3 startPoint, Vector3 startAnchor, Vector3 endAnchor, Vector3 endPoint)
		{
			Vector3 tangent = GetBezierTangent(t, startPoint, startAnchor, endAnchor, endPoint);
			Vector3 normal = Vector3.Cross(tangent, Vector3.up);
			return normal;
		}

		/// <summary> Clamps the given position pos to the box defined by boxCenter and boxSize </summary>
		public static Vector3 ClampToBox(Vector3 pos, Vector3 boxCenter, Vector3 boxSize) 
		{
			Vector3 boxMin = boxCenter - boxSize / 2;
			Vector3 boxMax = boxCenter + boxSize / 2;

			return new Vector3(
				Mathf.Clamp(pos.x, boxMin.x, boxMax.x),
				Mathf.Clamp(pos.y, boxMin.y, boxMax.y),
				Mathf.Clamp(pos.z, boxMin.z, boxMax.z)
			);
		}


		// thank you http://csharphelper.com/blog/2018/10/draw-an-archimedes-spiral-in-c/ for the below 2 functions!

		static Vector2 PolarToCartesian(float r, float theta)
		{
			return new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
		}


		public static Vector2 PointOnSpiral(float theta, float A, float angleOffset = 0)
		{
			float r = A * theta;

			// Convert to Cartesian coordinates.
			return PolarToCartesian(r, theta + angleOffset);
		}
		
		// thx unity forums
		// https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
		 public static float Remap (this float from, float fromMin, float fromMax, float toMin,  float toMax)
		{
			var fromAbs  =  from - fromMin;
			var fromMaxAbs = fromMax - fromMin;      
		
			var normal = fromAbs / fromMaxAbs;
	
			var toMaxAbs = toMax - toMin;
			var toAbs = toMaxAbs * normal;
	
			var to = toAbs + toMin;
		
			return to;
		}
	}
}