using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Surface))]
public class Destroyable : MonoBehaviour, IHittable, IDraggable
{
	[SerializeField]
	private int _health = 1;

	[SerializeField]
	private GameObject _unbroken;

	[SerializeField]
	private GameObject _broken;

	[SerializeField]
	private SpriteRenderer _unbrokenSpriteRenderer;

	[Header("Can Drag By Gravity Beam")]
	[SerializeField]
	private bool _canDrag;

	[SerializeField]
	private bool _doRotate = true;

	[SerializeField]
	private bool _doTurnOffIfInTriggerOfOtherBreakable;

	[Header("Falling")]
	[SerializeField]
	private bool _doFallDamage;

	[SerializeField]
	private float _fallDamageThreshold = 5f;

	private Surface _surface;

	private Collider2D _collider;

	private Rigidbody2D _rigidbody;

	private Material _unbrokenMaterial;

	private int _startHealth;

	public int Health => _health;

	public bool CanDrag => _canDrag;

	public bool DoRotate => _doRotate;

	public Action OnDestroyed { get; set; }

	private void Start()
	{
		_surface = GetComponent<Surface>();
		_collider = GetComponent<Collider2D>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_unbrokenMaterial = _unbrokenSpriteRenderer.material;
		_unbrokenMaterial.SetFloat("_MaxDamage", _health);
	}

	public void OnHit(Vector2 hitPoint, Vector2 hitDirection)
	{
		_health--;
		_unbrokenMaterial.SetFloat("_Damage", _health);
		if (_health <= 0)
		{
			Destroy();
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (_doFallDamage && !(other.relativeVelocity.magnitude < _fallDamageThreshold))
		{
			Destroy();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_doTurnOffIfInTriggerOfOtherBreakable && other.TryGetComponent<Destroyable>(out var _))
		{
			_canDrag = false;
			base.gameObject.layer = LayerMask.GetMask("Ignore Raycast");
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (_doTurnOffIfInTriggerOfOtherBreakable && other.TryGetComponent<Destroyable>(out var _))
		{
			_canDrag = true;
			base.gameObject.layer = LayerMask.GetMask("Default");
		}
	}

	private void Destroy()
	{
		_unbroken.SetActive(value: false);
		_broken.SetActive(value: true);
		_collider.enabled = false;
		_rigidbody.bodyType = RigidbodyType2D.Static;
		_surface.Destroy();
		OnDestroyed?.Invoke();
	}
}
