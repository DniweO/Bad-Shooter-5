using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private float _speed = 10f;

	[SerializeField]
	private int _maxBounces = 5;

	[SerializeField]
	private float _width = 0.1f;

	[SerializeField]
	private float _hitForce = 3f;

	[SerializeField]
	private TrailRenderer _trailRenderer;

	[SerializeField]
	private float _epsilon = 0.001f;

	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private float _waterPushupForce = 3f;

	private int _bounces;

	private void Reflect(Vector2 normal)
	{
		Vector2 vector = Vector2.Reflect(base.transform.right, normal);
		base.transform.right = vector;
	}

	private void Update()
	{
		float num = _speed * Time.deltaTime;
		_ = base.transform.position;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, base.transform.right, num + _width, _layerMask);
		WaterBehaviour();
		if ((bool)raycastHit2D.collider)
		{
			if (IsInsideCollider())
			{
				DestroyBullet();
				return;
			}
			Vector2 normal = raycastHit2D.normal;
			float num2 = raycastHit2D.distance - _width;
			float num3 = num - num2;
			base.transform.position += base.transform.right * num2;
			OnHit(raycastHit2D.collider, raycastHit2D.point);
			Reflect(normal);
			while (num3 > 0f && _bounces < _maxBounces)
			{
				raycastHit2D = Physics2D.Raycast(base.transform.position, base.transform.right, num3 + _width, _layerMask);
				if (IsInsideCollider())
				{
					DestroyBullet();
					break;
				}
				if ((bool)raycastHit2D.collider)
				{
					normal = raycastHit2D.normal;
					num2 = raycastHit2D.distance - _width;
					num3 -= num2;
					base.transform.position += base.transform.right * num2 * 0.99f;
					Reflect(normal);
					OnHit(raycastHit2D.collider, raycastHit2D.point);
					continue;
				}
				base.transform.position += base.transform.right * num3;
				num3 = 0f;
				break;
			}
		}
		else
		{
			base.transform.position += base.transform.right * num;
		}
	}

	private void OnHit(Collider2D collider, Vector2 hitPoint)
	{
		int num = 1;
		_trailRenderer.AddPosition(hitPoint);
		if ((bool)collider.attachedRigidbody)
		{
			collider.attachedRigidbody.AddForceAtPosition(base.transform.right * _hitForce, hitPoint, ForceMode2D.Impulse);
		}
		if (collider.TryGetComponent<Surface>(out var component))
		{
			component.OnHit(hitPoint, base.transform.right);
			num += component.GetSurfaceBulletDamage() - 1;
		}
		if (collider.TryGetComponent<IHittable>(out var component2))
		{
			component2.OnHit(hitPoint, base.transform.right);
		}
		_bounces += num;
		if (_bounces >= _maxBounces)
		{
			DestroyBullet();
		}
	}

	private bool IsInsideCollider()
	{
		Transform transform = base.transform;
		Vector3 up = transform.up;
		Vector3 right = transform.right;
		int num = 4;
		Vector2[] array = new Vector2[4]
		{
			up,
			-up,
			right,
			-right
		};
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if ((bool)Physics2D.Raycast(transform.position, array[i], _width, _layerMask))
			{
				num2++;
			}
		}
		return num2 == num;
	}

	private void WaterBehaviour()
	{
		Collider2D collider2D = Physics2D.OverlapPoint(base.transform.position, LayerMask.GetMask("Water"));
		if (!(collider2D == null) && collider2D.CompareTag("Water"))
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.MoveTowardsAngle(eulerAngles.z, 90f, _waterPushupForce * Time.deltaTime)));
		}
	}

	private void DestroyBullet()
	{
		_trailRenderer.transform.SetParent(null);
		Object.Destroy(_trailRenderer.gameObject, 0.5f);
		base.gameObject.SetActive(value: false);
	}
}
