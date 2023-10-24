using UnityEngine;

public class Surface : MonoBehaviour
{
	[SerializeField]
	private SurfaceMaterial _material;

	public void OnHit(Vector2 hitPoint, Vector2 hitDirection)
	{
		Object.Instantiate(_material.hitEffect, hitPoint, Quaternion.identity).transform.up = -hitDirection;
	}

	public int GetSurfaceBulletDamage()
	{
		return _material.bulletDamage;
	}

	public void Destroy()
	{
		Object.Instantiate(_material.destroyEffect, base.transform.position, base.transform.rotation);
	}
}
