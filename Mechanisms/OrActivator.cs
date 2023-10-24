using System;
using UnityEngine;

namespace Mechanisms
{
	public class OrActivator : Activator
	{
		[SerializeField]
		private Activator[] _activators;

		private int _activatedCount;

		private bool _isActivated;

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
			if (_activatedCount > 0 && !_isActivated)
			{
				_isActivated = true;
				OnActivated?.Invoke();
			}
		}

		private void SubtractActivation()
		{
			_activatedCount--;
			if (_activatedCount == 0 && _isActivated)
			{
				_isActivated = false;
				OnDeactivated?.Invoke();
			}
		}
	}
}
