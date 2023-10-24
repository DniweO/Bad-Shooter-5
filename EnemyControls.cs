using System;
using System.Collections;
using Mechanisms;
using UnityEngine;

public class EnemyControls : CharacterControls
{
	[SerializeField]
	private Transform[] _targets;

	[SerializeField]
	private float _delay = 2f;

	[SerializeField]
	private float _aimingDuration = 1f;

	[SerializeField]
	private bool _startOnAwake;

	[SerializeField]
	private Mechanisms.Activator _activator;

	private int _currentIndex;

	private bool _isAiming;

	private bool _isAlreadyAiming;

	private void Awake()
	{
		if (_startOnAwake)
		{
			StartAimingCoroutine();
		}
	}

	private void Update()
	{
		if (_isAiming)
		{
			Transform transform = _targets[_currentIndex];
			SetAimingPosition(transform.position);
			ChooseHand();
		}
	}

	private void OnEnable()
	{
		if ((bool)_activator)
		{
			Mechanisms.Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Combine(activator.OnActivated, new Action(StartAimingCoroutine));
			Mechanisms.Activator activator2 = _activator;
			activator2.OnDeactivated = (Action)Delegate.Combine(activator2.OnDeactivated, new Action(StopAiming));
		}
	}

	private void OnDisable()
	{
		if ((bool)_activator)
		{
			Mechanisms.Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Remove(activator.OnActivated, new Action(StartAimingCoroutine));
			Mechanisms.Activator activator2 = _activator;
			activator2.OnDeactivated = (Action)Delegate.Remove(activator2.OnDeactivated, new Action(StopAiming));
		}
	}

	private void StartAimingCoroutine()
	{
		_isAiming = true;
		StartCoroutine(AimAndShoot());
	}

	private void StopAiming()
	{
		_isAiming = false;
		_isAlreadyAiming = false;
		CancelAiming();
	}

	private IEnumerator AimAndShoot()
	{
		if (_isAlreadyAiming)
		{
			yield break;
		}
		while (_isAiming)
		{
			if (_targets.Length != 0)
			{
				_isAlreadyAiming = true;
				yield return new WaitForSeconds(_delay);
				StartAiming();
				yield return new WaitForSeconds(_aimingDuration);
				Shoot();
				_currentIndex = (_currentIndex + 1) % _targets.Length;
			}
			else
			{
				Debug.LogWarning("No targets assigned to the EnemyControls component.");
				StopAiming();
			}
		}
	}
}
