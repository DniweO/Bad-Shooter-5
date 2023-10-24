using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Mechanisms;
using UnityEngine;

public class ShopLevelController : Mechanisms.Activator
{
	[SerializeField]
	private Mechanisms.Activator[] _activators;

	[SerializeField]
	private SpriteRenderer[] _spriteRenderers;

	[SerializeField]
	private Color _activatedColor = Color.green;

	[SerializeField]
	private Color _deactivatedColor = Color.red;

	[Header("Delay and checkpoints")]
	[SerializeField]
	private float _approvalTime = 5f;

	[SerializeField]
	private GameObject[] _checkpointBoxes;

	[SerializeField]
	private GameObject[] _boxes;

	[SerializeField]
	private GameObject[] _bottles;

	[SerializeField]
	private Transform _bobSpawnPoint;

	[SerializeField]
	private Transform _bob;

	private bool[] _activatedPanels;

	private bool[] _approvedPanels;

	private Tweener _activationColorTweener;

	private void OnEnable()
	{
		_activatedPanels = new bool[_activators.Length];
		_approvedPanels = new bool[_activators.Length];
		for (int i = 0; i < _activators.Length; i++)
		{
			int index = i;
			Mechanisms.Activator obj = _activators[i];
			obj.OnActivated = (Action)Delegate.Combine(obj.OnActivated, (Action)delegate
			{
				OnActivate(index);
			});
			Mechanisms.Activator obj2 = _activators[i];
			obj2.OnDeactivated = (Action)Delegate.Combine(obj2.OnDeactivated, (Action)delegate
			{
				OnDeactivate(index);
			});
			_activatedPanels[i] = false;
			_approvedPanels[i] = false;
		}
		bool flag = false;
		int num = 0;
		for (int j = 0; j < _checkpointBoxes.Length; j++)
		{
			bool flag2 = PlayerPrefs.GetInt($"Box{j}", 0) == 1;
			_checkpointBoxes[j].SetActive(flag2);
			if (flag2)
			{
				flag = true;
				num++;
			}
		}
		if (flag)
		{
			_bob.position = _bobSpawnPoint.position;
			for (int k = 0; k < num; k++)
			{
				_boxes[k].SetActive(value: false);
				_bottles[k * 3].SetActive(value: false);
				_bottles[k * 3 + 1].SetActive(value: false);
				_bottles[k * 3 + 2].SetActive(value: false);
			}
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < _activators.Length; i++)
		{
			int index = i;
			Mechanisms.Activator obj = _activators[i];
			obj.OnActivated = (Action)Delegate.Remove(obj.OnActivated, (Action)delegate
			{
				OnActivate(index);
			});
			Mechanisms.Activator obj2 = _activators[i];
			obj2.OnDeactivated = (Action)Delegate.Remove(obj2.OnDeactivated, (Action)delegate
			{
				OnDeactivate(index);
			});
		}
	}

	private void OnActivate(int index)
	{
		_spriteRenderers[index].color = new Color(_activatedColor.r, _activatedColor.g, _activatedColor.b, 0f);
		_activationColorTweener.Kill();
		_activationColorTweener = _spriteRenderers[index].DOColor(_activatedColor, _approvalTime);
		CommitActivationStatus(index);
	}

	private void OnDeactivate(int index)
	{
		_activationColorTweener.Kill();
		_activationColorTweener = _spriteRenderers[index].DOColor(_deactivatedColor, 0.5f);
		CancelActivationStatus(index);
	}

	private void CommitActivationStatus(int index)
	{
		_activatedPanels[index] = true;
		StartCoroutine(ActivationCoroutine(index, delegate
		{
			Debug.Log($"Box with {index} is activated");
			PlayerPrefs.SetInt($"Box{index}", 1);
			PlayerPrefs.Save();
			CheckIfLevelIsCompleted();
		}));
	}

	private void CancelActivationStatus(int index)
	{
		_activatedPanels[index] = false;
		_approvedPanels[index] = false;
		Debug.Log($"Box with {index} is deactivated");
		PlayerPrefs.SetInt($"Box{index}", 0);
	}

	private void CheckIfLevelIsCompleted()
	{
		if (_approvedPanels.All((bool x) => x))
		{
			OnActivated?.Invoke();
		}
	}

	private IEnumerator ActivationCoroutine(int index, Action onApproved)
	{
		yield return new WaitForSeconds(_approvalTime);
		_approvedPanels[index] = _activatedPanels[index];
		if (_approvedPanels[index])
		{
			onApproved?.Invoke();
		}
	}
}
