using UnityEngine;

namespace Mechanisms
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Lever : Activator, IHittable
	{
		[SerializeField]
		private float _targetAngle = 45f;

		[SerializeField]
		private Spring _spring;

		private bool _isPressed;

		private Rigidbody2D _rigidbody;

		public bool IsPressed => _isPressed;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			if (!_spring.isFinished)
			{
				_spring.Update(Time.deltaTime);
				base.transform.localRotation = Quaternion.Euler(0f, 0f, _spring.value);
				_rigidbody.MoveRotation(base.transform.rotation);
			}
		}

		public void OnHit(Vector2 hitPoint, Vector2 hitDirection)
		{
			Vector3 vector = base.transform.InverseTransformDirection(hitDirection);
			if (vector.x > 0f)
			{
				_isPressed = true;
				OnActivated?.Invoke();
			}
			if (vector.x < 0f)
			{
				_isPressed = false;
				OnDeactivated?.Invoke();
			}
			float target = (_isPressed ? (0f - _targetAngle) : _targetAngle);
			_spring.target = target;
		}
	}
}
