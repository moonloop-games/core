using UnityEngine;

namespace Moonloop.Core {

public static class Gizmos2D
{
    public static void DrawCircle2D(Vector2 pos, float radius)
    {
		Color color = Gizmos.color;
		var degreesInterval = 10;
		for (int i = 0; i < 360; i+= degreesInterval) 
        {
			Gizmos.DrawLine(pos + PointAtAngle2D(i, radius), pos + PointAtAngle2D(i + degreesInterval, radius));
		}
    }

    static Vector2 PointAtAngle2D(float angle, float radius) 
    {
		float radians = Mathf.Deg2Rad * angle;
		float x = radius * Mathf.Cos(radians);
		float y = radius * Mathf.Sin(radians);
		return new Vector3(x, y, 0);
	}

    public static void DrawArrow2D(Vector2 pos, Vector2 dir, float arrowHeadLength = 0.5f, float arrowHeadAngle = 20.0f)
	{
		if (dir.sqrMagnitude < 0.01f) return;
		Gizmos.DrawRay(pos, dir);
		Vector2 right = Quaternion.LookRotation(dir) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * Vector3.forward;
		Vector2 left = Quaternion.LookRotation(dir) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * Vector3.forward;
		Gizmos.DrawRay(pos + dir, right * arrowHeadLength);
		Gizmos.DrawRay(pos + dir, left * arrowHeadLength);
	}

	public static void DrawMagnitudeArrow2D(Vector2 pos, Vector2 direction) 
	{
		DrawCircle2D(pos, direction.magnitude);
        DrawArrow2D(pos, direction);
	}
}
}