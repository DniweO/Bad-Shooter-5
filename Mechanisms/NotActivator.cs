using System;
using UnityEngine;

namespace Mechanisms
{
	public class NotActivator : Activator
	{
		[SerializeField]
		private Activator _activator;

		private void OnEnable()
		{
			Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Combine(activator.OnActivated, new Action(Deactivate));
			Activator activator2 = _activator;
			activator2.OnDeactivated = (Action)Delegate.Combine(activator2.OnDeactivated, new Action(Activate));
		}

		private void OnDisable()
		{
			Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Remove(activator.OnActivated, new Action(Deactivate));
			Activator activator2 = _activator;
			activator2.OnDeactivated = (Action)Delegate.Remove(activator2.OnDeactivated, new Action(Activate));
		}

		private void Activate()
		{
			OnActivated?.Invoke();
			Debug.Log("Not Activated");
		}

		private void Deactivate()
		{
			OnDeactivated?.Invoke();
			Debug.Log("Not Deactivated");
		}
	}
}
