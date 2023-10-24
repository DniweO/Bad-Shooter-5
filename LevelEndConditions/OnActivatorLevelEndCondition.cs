using System;
using Mechanisms;
using UnityEngine;

namespace LevelEndConditions
{
	public class OnActivatorLevelEndCondition : LevelEndCondition
	{
		[SerializeField]
		private Mechanisms.Activator _activator;

		public override Action OnConditionMet { get; set; }

		private void OnEnable()
		{
			Mechanisms.Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Combine(activator.OnActivated, new Action(OnActivated));
		}

		private void OnDisable()
		{
			Mechanisms.Activator activator = _activator;
			activator.OnActivated = (Action)Delegate.Remove(activator.OnActivated, new Action(OnActivated));
		}

		private void OnActivated()
		{
			OnConditionMet?.Invoke();
		}
	}
}
