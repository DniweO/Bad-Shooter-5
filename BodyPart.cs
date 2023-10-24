using UnityEngine;

public class BodyPart : MonoBehaviour, IHittable
{
	[SerializeField]
	private FloatingCharacter _floatingCharacter;

	[SerializeField]
	private float _hitForce = 50f;

	private Rigidbody2D _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	public void OnHit(Vector2 hitPoint, Vector2 hitDirection)
	{
		_floatingCharacter.IsActive = false;
		_rigidbody.AddForceAtPosition(hitDirection * _hitForce, hitPoint, ForceMode2D.Impulse);
	}
}
