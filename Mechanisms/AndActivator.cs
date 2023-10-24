using System;
using UnityEngine;

namespace Mechanisms
{
	public class AndActivator : Activator
	{
		[SerializeField]
		private Activator[] _activators;

		private int _activatedCount;

		private void OnEnable()
		{
			Activator[] activators = _activators;
			foreach (Activator obj in activators)
			{
				obj.OnActivated = (Action)Delegate.Combine(obj.OnActivated, new Action(AddActivation));
				obj.OnDeactivated = (Action)Delegate.Combine(obj.OnDeactivated, new Action(SubtractActivation));
			}
		}

		private void OnDisable()
		{
			Activator[] activators = _activators;
			foreach (Activator obj in activators)
			{
				obj.OnActivated = (Action)Delegate.Remove(obj.OnActivated, new Action(AddActivation));
				obj.OnDeactivated = (Action)Delegate.Remove(obj.OnDeactivated, new Action(SubtractActivation));
			}
		}

		private void AddActivation()
		{
			_activatedCount++;
			Debug.Log($"And Activated {_activatedCount}/{_activators.Length}");
			if (_activatedCount == _activators.Length)
			{
				OnActivated?.Invoke();
				Debug.Log("And Activated");
			}
		}

		private void SubtractActivation()
		{
			_activatedCount--;
			if (_activatedCount != _activators.Length)
			{
				OnDeactivated?.Invoke();
				Debug.Log("And Deactivated");
			}
		}
	}
}
