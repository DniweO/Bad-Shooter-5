using System.Collections;
using UnityEngine;

public class Eyes : MonoBehaviour
{
	private enum Eye
	{
		Left = 0,
		Right = 1
	}

	[SerializeField]
	private SpriteRenderer _leftEye;

	[SerializeField]
	private SpriteRenderer _rightEye;

	[SerializeField]
	private GameObject _closedLeftEye;

	[SerializeField]
	private GameObject _closedRightEye;

	[SerializeField]
	private float _maxDistance = 20f;

	[SerializeField]
	private float _blinkInterval = 3f;

	[SerializeField]
	private float _blinkDuration = 0.1f;

	[SerializeField]
	private FloatingCharacter _floatingCharacter;

	[SerializeField]
	private AimingController _aimingController;

	private Material _leftEyeMaterial;

	private Material _rightEyeMaterial;

	private bool _isLeftEyeClosed;

	private bool _isRightEyeClosed;

	private bool _isAiming;

	private void Start()
	{
		_leftEyeMaterial = _leftEye.material;
		_rightEyeMaterial = _rightEye.material;
		StartCoroutine(BlinkCoroutine());
	}

	private void Update()
	{
		if (_floatingCharacter.IsNotActive)
		{
			CloseEyes();
			return;
		}
		if (_aimingController.isAiming)
		{
			bool num = _aimingController.currentWrist == Wrists.Left;
			Eye eye = (num ? Eye.Right : Eye.Left);
			Eye eye2 = ((!num) ? Eye.Right : Eye.Left);
			CloseEye(eye);
			OpenEye(eye2);
			_isAiming = true;
		}
		else if (_isAiming)
		{
			OpenEyes();
			_isAiming = false;
		}
		Vector3 aimTargetPosition = _aimingController.AimTargetPosition;
		SetEyePosition(aimTargetPosition, Eye.Left);
		SetEyePosition(aimTargetPosition, Eye.Right);
	}

	private void SetEyePosition(Vector3 worldPosition, Eye eye)
	{
		Material obj = ((eye == Eye.Left) ? _leftEyeMaterial : _rightEyeMaterial);
		Vector3 position = ((eye == Eye.Left) ? _leftEye.transform : _rightEye.transform).position;
		float num = 0.5f;
		Vector3 normalized = (worldPosition - position).normalized;
		float num2 = Mathf.Clamp01(Vector3.Distance(worldPosition, position) / _maxDistance);
		Vector3 vector = normalized * num2 * num;
		obj.SetVector("_Position", vector);
	}

	private void CloseEye(Eye eye)
	{
		((eye == Eye.Left) ? _closedLeftEye : _closedRightEye).SetActive(value: true);
		if (eye == Eye.Left)
		{
			_isLeftEyeClosed = true;
		}
		else
		{
			_isRightEyeClosed = true;
		}
	}

	private void OpenEye(Eye eye)
	{
		((eye == Eye.Left) ? _closedLeftEye : _closedRightEye).SetActive(value: false);
		if (eye == Eye.Left)
		{
			_isLeftEyeClosed = false;
		}
		else
		{
			_isRightEyeClosed = false;
		}
	}

	private void CloseEyes()
	{
		CloseEye(Eye.Left);
		CloseEye(Eye.Right);
	}

	private void OpenEyes()
	{
		OpenEye(Eye.Left);
		OpenEye(Eye.Right);
	}

	private IEnumerator Blink()
	{
		if (!_isLeftEyeClosed)
		{
			_closedLeftEye.SetActive(value: true);
		}
		if (!_isRightEyeClosed)
		{
			_closedRightEye.SetActive(value: true);
		}
		yield return new WaitForSeconds(_blinkDuration);
		if (!_isLeftEyeClosed)
		{
			_closedLeftEye.SetActive(value: false);
		}
		if (!_isRightEyeClosed)
		{
			_closedRightEye.SetActive(value: false);
		}
	}

	private IEnumerator BlinkCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(_blinkInterval);
			if (_floatingCharacter.IsNotActive)
			{
				break;
			}
			yield return Blink();
		}
	}
}
