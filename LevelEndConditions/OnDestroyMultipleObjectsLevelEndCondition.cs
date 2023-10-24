using System;
using UnityEngine;

namespace LevelEndConditions
{
	public class OnDestroyMultipleObjectsLevelEndCondition : LevelEndCondition
	{
		[SerializeField]
		private Destroyable[] _objects;

		private int _conditionMetCount;

		private bool _isConditionMet;

		public override Action OnConditionMet { get; set; }

		private void OnEnable()
		{
			Destroyable[] objects = _objects;
			foreach (Destroyable obj in objects)
			{
				obj.OnDestroyed = (Action)Delegate.Combine(obj.OnDestroyed, new Action(OnConditionMetOnce));
			}
		}

		private void OnDisable()
		{
			Destroyable[] objects = _objects;
			foreach (Destroyable obj in objects)
			{
				obj.OnDestroyed = (Action)Delegate.Remove(obj.OnDestroyed, new Action(OnConditionMetOnce));
			}
		}

		private void OnConditionMetOnce()
		{
			_conditionMetCount++;
			if (_conditionMetCount >= _objects.Length && !_isConditionMet)
			{
				_isConditionMet = true;
				OnConditionMet?.Invoke();
			}
		}
	}
}
