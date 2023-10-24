using UnityEngine;

namespace Mechanisms
{
	public class Button : Activator, IHittable
	{
		[SerializeField]
		private Transform _button;

		[SerializeField]
		private Transform _buttonBase;

		[SerializeField]
		private Spring _pushSpring;

		[SerializeField]
		private Spring _squishSpring;

		[SerializeField]
		private float _buttonPushLocalY = 0.1f;

		[SerializeField]
		private float _buttonSquishAmount = 0.1f;

		private bool _isPushing;

		private bool _isPressed;

		public bool IsPressed => _isPressed;

		public void OnHit(Vector2 hitPoint, Vector2 hitDirection)
		{
			if (!_isPressed)
			{
				_isPressed = true;
				_isPushing = true;
				_pushSpring.target = _buttonPushLocalY;
				_squishSpring.target = _buttonSquishAmount;
				OnActivated?.Invoke();
			}
		}

		private void Update()
		{
			if (_isPushing)
			{
				_button.localPosition = new Vector3(0f, _pushSpring.value, 0f);
				_button.localScale = new Vector3(1f + _squishSpring.value, 1f - _squishSpring.value, 1f);
				_buttonBase.localScale = new Vector3(1f + _squishSpring.value, 1f - _squishSpring.value, 1f);
				_pushSpring.Update(Time.deltaTime);
				_squishSpring.Update(Time.deltaTime);
				if (_pushSpring.isFinished && _squishSpring.isFinished)
				{
					_isPushing = false;
				}
			}
		}
	}
}
