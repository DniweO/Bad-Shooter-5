using UnityEngine;

public class BezierCurve
{
	public Vector2 StartPoint;

	public Vector2 EndPoint;

	public Vector2 ControlPoint;

	public Vector2 GetPoint(float t)
	{
		return Vector2.Lerp(Vector2.Lerp(StartPoint, ControlPoint, t), Vector2.Lerp(ControlPoint, EndPoint, t), t);
	}
}
