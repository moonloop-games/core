using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonloop.Core {

public static class Gizmo
{
		public static void DrawCircle(Vector3 pos, float radius, bool fill = false)
		{
			Color color = Gizmos.color;
			for (int i = 0; i < 360; i+= 20) {
				Gizmos.DrawLine(pos + PointAtAngle(i, radius), pos + PointAtAngle(i + 20, radius));
			}

			if (fill)
			{
				Gizmos.color = new Color(color.r, color.g, color.b, color.a * .3f);
				for (float r = 0.5f; r < radius; r += .5f)
				{
					DrawCircle(pos, r);
				}
				Gizmos.color = color;
			}
		}

		public static void DrawCone(Vector3 pos, Vector3 dir, float range, float angle)
		{
			Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0,  angle, 0) * Vector3.forward;
			Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0,  -angle, 0) * Vector3.forward;
			Vector3 up = Quaternion.LookRotation(dir) * Quaternion.Euler(angle, 0, 0) * Vector3.forward;
			Vector3 down = Quaternion.LookRotation(dir) * Quaternion.Euler(-angle, 0, 0) * Vector3.forward;
			
			Gizmos.DrawRay(pos, right * range);
			Gizmos.DrawRay(pos, left * range);
			Gizmos.DrawRay(pos, up * range);
			Gizmos.DrawRay(pos, down * range);

			// calculate the radius at the end of the range
			float radius = Mathf.Tan(Mathf.Deg2Rad * angle) * range;

			Gizmos.DrawWireSphere(pos + dir * range, radius);
		}

		public static void DrawArrow(Vector3 pos, Vector3 dir, float arrowHeadLength = 0.5f, float arrowHeadAngle = 20.0f)
		{
			if (dir.sqrMagnitude < 0.01f) return;
			Gizmos.DrawRay(pos, dir);
			Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
			Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
			Gizmos.DrawRay(pos + dir, right * arrowHeadLength);
			Gizmos.DrawRay(pos + dir, left * arrowHeadLength);
		}

		static Vector3 PointAtAngle(float angle, float radius) 
		{
			float radians = Mathf.Deg2Rad * angle;
			float x = radius * Mathf.Cos(radians);
			float z = radius * Mathf.Sin(radians);
			return new Vector3(x, 0, z);
		}

		public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius)
		{
			Gizmos.DrawWireSphere(point0, radius);
			Gizmos.DrawWireSphere(point1, radius);

			// draw lines to connect the two spheres
			Gizmos.DrawLine(point0 + Vector3.up * radius, point1 + Vector3.up * radius);
			Gizmos.DrawLine(point0 - Vector3.up * radius, point1 - Vector3.up * radius);
			Gizmos.DrawLine(point0 + Vector3.right * radius, point1 + Vector3.right * radius);
			Gizmos.DrawLine(point0 - Vector3.right * radius, point1 - Vector3.right * radius);
			Gizmos.DrawLine(point0 + Vector3.forward * radius, point1 + Vector3.forward * radius);
			Gizmos.DrawLine(point0 - Vector3.forward * radius, point1 - Vector3.forward * radius);
		}

		public static void DrawBezier(Vector3 point1, Vector3 anchor1, Vector3 anchor2, Vector3 point2) 
		{
			Vector3 prevPoint = Math.GetBezier(0, point1, anchor1, anchor2, point2);
			for (float i = .02f; i < 1; i += .02f)
			{
				Vector3 thisPoint = Math.GetBezier(i, point1, anchor1, anchor2, point2);
				Gizmos.DrawLine(prevPoint, thisPoint);
				prevPoint = thisPoint;
			}
		}

		/// <summary> Draws a parabolic arc between two points </summary>
		public static void DrawArc(Vector3 pt1, Vector3 pt2, float arcHeight = 10) 
		{
			for (int i = 0; i < 30; i++) 
			{
				float t = i / 30f;
				Vector3 pos = Math.Parabola(pt1, arcHeight, pt2, t);
				Vector3 pos2 = Math.Parabola(pt1, arcHeight, pt2, t + .03f);

				Gizmos.DrawLine(pos, pos2);
			}
		}

		public static void DrawGunGizmo(Vector3 pos, Vector3 dir, float radius = .25f)
		{
			// get a rotation that looks in the direction
			Quaternion rot = Quaternion.LookRotation(dir);

			// draw a wire sphere at the origin
			Gizmos.DrawWireSphere(pos, radius);

			Gizmos.DrawRay(pos + rot * Vector3.up * radius, dir * 2);
			Gizmos.DrawRay(pos + rot * Vector3.down * radius, dir * 2);
			Gizmos.DrawRay(pos + rot * Vector3.right * radius, dir * 2);
			Gizmos.DrawRay(pos + rot * Vector3.left * radius, dir * 2);
		}
	}
}
