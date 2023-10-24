using UnityEngine;

public interface IHittable
{
	void OnHit(Vector2 hitPoint, Vector2 hitDirection);
}
