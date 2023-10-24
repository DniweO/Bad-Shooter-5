using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[SerializeField]
	private float _radius = 1f;

	[SerializeField]
	private float _force = 1f;

	[SerializeField]
	private int _damage = 1;

	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private float _damageDealingDuration = 0.35f;

	private void Start()
	{
		Explode();
	}

	private void Explode()
	{
		Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, _radius, _layerMask);
		foreach (Collider2D obj in array)
		{
			Vector3 vector = obj.transform.position - base.transform.position;
			float num = 1f - vector.magnitude / _radius;
			float num2 = num * _force;
			if (obj.TryGetComponent<Rigidbody2D>(out var component))
			{
				component.AddForce(vector.normalized * num2, ForceMode2D.Impulse);
			}
			if (obj.TryGetComponent<IHittable>(out var component2))
			{
				StartCoroutine(DamageDealing(component2, base.transform, num));
			}
		}
	}

	private IEnumerator DamageDealing(IHittable hittable, Transform position, float forceRatio)
	{
		for (int i = 0; (float)i < (float)_damage * forceRatio; i++)
		{
			hittable.OnHit(position.position, Vector2.zero);
			yield return new WaitForSeconds(_damageDealingDuration / (float)_damage);
		}
	}
}
