using UnityEngine;

namespace Mechanisms
{
	[RequireComponent(typeof(Collider2D))]
	public class WeightPlatform : Activator
	{
		[SerializeField]
		private Transform _top;

		[SerializeField]
		private Transform _bottom;

		[SerializeField]
		private float _activationDistance = 0.1f;

		[SerializeField]
		private float _maxDistance = 0.5f;

		private Rigidbody2D _rigidbody;

		private bool _isPressed;

		public bool IsPressed => _isPressed;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.attachedRigidbody == null)
			{
				return;
			}
			float num = Vector2.Distance(_top.position, _bottom.position);
			if (num > _maxDistance)
			{
				_rigidbody.MovePosition(_bottom.position + (_top.position - _bottom.position).normalized * (_maxDistance * 0.75f));
			}
			if (num < _activationDistance)
			{
				if (!_isPressed)
				{
					_isPressed = true;
					OnActivated?.Invoke();
				}
			}
			else if (_isPressed)
			{
				_isPressed = false;
				OnDeactivated?.Invoke();
			}
		}
	}
}
