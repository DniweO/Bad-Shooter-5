using System;
using System.Collections.Generic;
using System.Linq;
using LevelEndConditions;
using UnityEngine;
using Zenject;

public class FloatingCharacter : MonoBehaviour
{
	[SerializeField]
	private float _speed = 1f;

	[SerializeField]
	private float _acceleration = 10f;

	[SerializeField]
	private float _deceleration = 15f;

	[SerializeField]
	private float _standingHeight = 1f;

	[SerializeField]
	private float _crouchingHeight = 0.5f;

	[SerializeField]
	private float _groundedBuffer = 0.1f;

	[SerializeField]
	private float _standingSpringStrength = 1f;

	[SerializeField]
	private float _standingSpringDamping = 0.75f;

	[SerializeField]
	private LayerMask _standingRaycastMask;

	[SerializeField]
	private ContactFilter2D _wallsRaycastFilter;

	[SerializeField]
	private float _feetAnimationSpeed = 1f;

	[SerializeField]
	private float _feetAnimationPower = 4f;

	[SerializeField]
	private float _maxDownBodyMovement = 0.5f;

	[Header("References")]
	[SerializeField]
	private Rigidbody2D _hips;

	[SerializeField]
	private Rigidbody2D _head;

	[SerializeField]
	private Rigidbody2D _rightFoot;

	[SerializeField]
	private Rigidbody2D _leftFoot;

	[SerializeField]
	private FloatingCharacterControls _controls;

	private bool _isCrouching;

	private bool _isGrounded;

	private bool _isActive;

	private float _currentSpeed;

	private int _currentDirection;

	private float _horizontalVelocity;

	private float _standingHeightDownOffset;

	private LevelEndChecker _levelEndChecker;

	public Action OnDeath;

	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			_hips.isKinematic = value;
			_isActive = value;
			if (!value)
			{
				OnDeath?.Invoke();
			}
		}
	}

	public bool IsNotActive => !IsActive;

	[Inject]
	private void Construct(LevelEndChecker levelEndChecker)
	{
		_levelEndChecker = levelEndChecker;
	}

	private void Start()
	{
		IsActive = true;
		Physics2D.IgnoreCollision(_leftFoot.GetComponent<Collider2D>(), _rightFoot.GetComponent<Collider2D>());
	}

	private void FixedUpdate()
	{
		if (IsNotActive)
		{
			return;
		}
		if (_levelEndChecker.IsLevelEnded)
		{
			ProcessStanding();
			return;
		}
		float horizontal = _controls.Horizontal;
		_isCrouching = _controls.IsCrouching;
		_currentDirection = ((horizontal != 0f) ? ((int)Mathf.Sign(horizontal)) : _currentDirection);
		if (horizontal != 0f)
		{
			_currentSpeed = Mathf.Min(_currentSpeed + _acceleration * Time.fixedDeltaTime, _speed);
		}
		else
		{
			_currentSpeed = Mathf.Max(_currentSpeed - _deceleration * Time.fixedDeltaTime, 0f);
		}
		_horizontalVelocity = (float)_currentDirection * _currentSpeed;
		ProcessStanding();
		AnimateFeet();
	}

	private void ProcessStanding()
	{
		float num = (_isCrouching ? _crouchingHeight : _standingHeight);
		num += _standingHeightDownOffset;
		RaycastHit2D[] source = Physics2D.RaycastAll(_hips.position, Vector2.down, _standingHeight * 2f, _standingRaycastMask);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(_hips.position, Vector2.up, _standingHeight + _crouchingHeight, _standingRaycastMask);
		RaycastHit2D raycastHit2D2 = source.FirstOrDefault((RaycastHit2D x) => !x.collider.isTrigger);
		if (raycastHit2D.collider != null)
		{
			_isCrouching = true;
			num = _crouchingHeight;
		}
		if (_isGrounded)
		{
			_isGrounded = raycastHit2D2.collider != null && raycastHit2D2.distance <= num + _groundedBuffer;
		}
		else
		{
			_isGrounded = raycastHit2D2.collider != null && raycastHit2D2.distance <= num;
		}
		float num2 = 0f;
		if (_isGrounded)
		{
			num2 = _hips.velocity.y;
			float num3 = 0f - (raycastHit2D2.distance - num);
			num2 += (num3 * _standingSpringStrength - _hips.velocity.y * _standingSpringDamping) * Time.fixedDeltaTime;
		}
		else
		{
			float y = Physics2D.gravity.y;
			num2 = _hips.velocity.y;
			num2 += y * Time.fixedDeltaTime;
		}
		Vector2 velocity = new Vector2(_horizontalVelocity, num2);
		List<RaycastHit2D> list = new List<RaycastHit2D>(1);
		if (_hips.Cast(new Vector2(_horizontalVelocity, 0f), _wallsRaycastFilter, list, velocity.magnitude * Time.fixedDeltaTime + 0.1f) > 0)
		{
			velocity.x = 0f;
			if (list[0].collider.attachedRigidbody != null && !list[0].collider.attachedRigidbody.isKinematic)
			{
				List<RaycastHit2D> list2 = new List<RaycastHit2D>(1);
				BodyPart component;
				if (list[0].collider.attachedRigidbody.Cast(new Vector2(_horizontalVelocity, 0f), _wallsRaycastFilter, list2, velocity.magnitude * Time.fixedDeltaTime + 0.1f) == 0)
				{
					velocity.x = _horizontalVelocity * 0.5f;
				}
				else if (list2[0].collider.TryGetComponent<BodyPart>(out component))
				{
					velocity.x = _horizontalVelocity * 0.5f;
				}
			}
		}
		_hips.velocity = velocity;
	}

	private void AnimateFeet()
	{
		if (_currentSpeed > 0f)
		{
			float num = _currentSpeed / _speed;
			float f = Time.time * _feetAnimationSpeed * (float)_currentDirection;
			float num2 = _feetAnimationPower * num;
			float num3 = Mathf.Sin(f) * 0.75f * num2;
			float num4 = Mathf.Cos(f) * num2;
			float y = num3;
			float x = num4;
			_standingHeightDownOffset = (Mathf.Sin(Time.time * _feetAnimationSpeed) - 1f) * _maxDownBodyMovement * num;
			_leftFoot.AddForce(new Vector2(num4, num3));
			_rightFoot.AddForce(new Vector2(x, y));
		}
		else
		{
			Vector2 force = (_isCrouching ? Vector2.up : (Vector2.down * _feetAnimationPower));
			_leftFoot.AddForce(force);
			_rightFoot.AddForce(force);
			_standingHeightDownOffset = 0f;
		}
	}
}
