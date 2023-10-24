using System;
using UnityEngine;

namespace LevelEndConditions
{
	public class OnDestroyObjectLevelEndCondition : LevelEndCondition
	{
		[SerializeField]
		private Destroyable _object;

		private bool _isConditionMet;

		public override Action OnConditionMet { get; set; }

		private void OnEnable()
		{
			Destroyable @object = _object;
			@object.OnDestroyed = (Action)Delegate.Combine(@object.OnDestroyed, new Action(OnObjectDestroyed));
		}

		private void OnDisable()
		{
			Destroyable @object = _object;
			@object.OnDestroyed = (Action)Delegate.Remove(@object.OnDestroyed, new Action(OnObjectDestroyed));
		}

		private void OnObjectDestroyed()
		{
			if (!_isConditionMet)
			{
				_isConditionMet = true;
				OnConditionMet?.Invoke();
			}
		}
	}
}
