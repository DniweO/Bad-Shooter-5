using LevelEndConditions;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(FloatingCharacter))]
public class AimingController : MonoBehaviour
{
	[HideInInspector]
	public bool isAiming;

	[HideInInspector]
	public Wrists currentWrist;

	[Header("Aiming")]
	[SerializeField]
	private Wrist _rightWrist;

	[SerializeField]
	private Wrist _leftWrist;

	[SerializeField]
	private Pistol _pistol;

	[SerializeField]
	private Transform _aimTarget;

	[SerializeField]
	private Spring _wristSpring;

	[SerializeField]
	private float _aimingPower = 30f;

	[Header("Head Rotation")]
	[SerializeField]
	private Rigidbody2D _head;

	[SerializeField]
	private float _headRotationLimit = 15f;

	[SerializeField]
	private float _deadAngle = 5f;

	private float _targetAngle;

	private FloatingCharacter _floatingCharacter;

	private LevelEndChecker _levelEndChecker;

	public Vector3 AimTargetPosition => _aimTarget.position;

	private bool IsNotActive
	{
		get
		{
			if (!_floatingCharacter.IsNotActive)
			{
				return _levelEndChecker.IsLevelEnded;
			}
			return true;
		}
	}

	[Inject]
	private void Construct(LevelEndChecker levelEndChecker)
	{
		_levelEndChecker = levelEndChecker;
	}

	private void Start()
	{
		_floatingCharacter = GetComponent<FloatingCharacter>();
		Physics2D.IgnoreCollision(_rightWrist.rigidbody.GetComponent<Collider2D>(), _pistol.GetComponent<Collider2D>());
		Physics2D.IgnoreCollision(_leftWrist.rigidbody.GetComponent<Collider2D>(), _pistol.GetComponent<Collider2D>());
	}

	public void SetRulingHand(Wrists wrist)
	{
		if (!IsNotActive)
		{
			currentWrist = wrist;
			Wrist wrist2 = ((wrist == Wrists.Right) ? _rightWrist : _leftWrist);
			Transform obj = _pistol.transform;
			obj.SetParent(wrist2.pistolPosition);
			obj.localPosition = Vector3.zero;
			obj.localRotation = Quaternion.identity;
			obj.localScale = Vector3.one;
		}
	}

	private void FixedUpdate()
	{
		if (!IsNotActive && isAiming)
		{
			Wrist wrist = ((currentWrist == Wrists.Right) ? _rightWrist : _leftWrist);
			Vector3 vector = _aimTarget.position - wrist.pistolPosition.position;
			vector.Normalize();
			wrist.rigidbody.AddForce(vector * _aimingPower);
			float targetAngle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			_targetAngle = targetAngle;
			_wristSpring.target = _targetAngle;
			_wristSpring.Update(Time.fixedDeltaTime);
			wrist.rigidbody.MoveRotation(_wristSpring.value);
			Vector3 vector2 = _aimTarget.position - _head.transform.position;
			vector2.Normalize();
			float num = 0f;
			if (vector2.x < 0f)
			{
				vector2 = -vector2;
			}
			num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			if (!(Mathf.Abs(num) > _deadAngle))
			{
				num = Mathf.Clamp(num, 0f - _headRotationLimit, _headRotationLimit);
				_head.MoveRotation(num);
			}
		}
	}
}
