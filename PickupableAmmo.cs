using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public class PickupableAmmo : MonoBehaviour
{
	[SerializeField]
	private float _pickupDuration = 0.5f;

	private Animator _animator;

	private bool _isPickedUp;

	private Collider2D _collider;

	public void Pickup(Transform parent, Action onPickup)
	{
		if (!_isPickedUp)
		{
			_isPickedUp = true;
			_animator.enabled = false;
			_collider.enabled = false;
			StartCoroutine(MovingToPlayer(parent, onPickup));
		}
	}

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_collider = GetComponent<Collider2D>();
	}

	private float EaseInQuart(float x)
	{
		return x * x * x * x;
	}

	private IEnumerator MovingToPlayer(Transform parent, Action onPickup)
	{
		float timer = _pickupDuration;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			float t = EaseInQuart(1f - timer / _pickupDuration);
			base.transform.position = Vector3.Lerp(base.transform.position, parent.position, t);
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one * 0.5f, t);
			yield return null;
		}
		onPickup?.Invoke();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
