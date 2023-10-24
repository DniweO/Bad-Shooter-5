using System;
using UnityEngine;

namespace Mechanisms
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class SlidingDoor : MonoBehaviour
	{
		[SerializeReference]
		private Activator _activator;

		[SerializeField]
		private Vector2 _direction;

		[SerializeField]
		private float _distance;

		[SerializeField]
		private Spring _spring;

		[SerializeField]
		private ContactFilter2D _stopFilter;

		private Rigidbody2D _rigidbody;

		private Vector2 _startPosition;

		private void Start()
		{
			_startPosition = base.transform.position;
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void OnEnable()
		{
			Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Combine(activator.OnActivated, new Action(Open));
			Activator activator2 = _activator;
			activator2.OnDeactivated = (Action)Delegate.Combine(activator2.OnDeactivated, new Action(Close));
		}

		private void OnDisable()
		{
			Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Remove(activator.OnActivated, new Action(Open));
			Activator activator2 = _activator;
			activator2.OnDeactivated = (Action)Delegate.Remove(activator2.OnDeactivated, new Action(Close));
		}

		private void Update()
		{
			if (!_spring.isFinished)
			{
				Vector2 vector = _startPosition + _direction * (_spring.value + 0.1f) - _rigidbody.position;
				if (_rigidbody.Cast(vector.normalized, _stopFilter, new RaycastHit2D[1], vector.magnitude) == 0)
				{
					_spring.Update(Time.deltaTime);
					_rigidbody.MovePosition(_startPosition + _direction * _spring.value);
				}
			}
		}

		private void Open()
		{
			_spring.target = _distance;
		}

		private void Close()
		{
			_spring.target = 0f;
		}
	}
}
