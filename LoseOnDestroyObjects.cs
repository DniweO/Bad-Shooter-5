using System;
using LevelEndConditions;
using UnityEngine;

public class LoseOnDestroyObjects : LevelEndCondition
{
	[SerializeField]
	private Destroyable[] _destroyables;

	[SerializeField]
	private int _countToLose = 1;

	private bool _isConditionMet;

	public override Action OnConditionMet { get; set; }

	private void OnEnable()
	{
		Destroyable[] destroyables = _destroyables;
		foreach (Destroyable obj in destroyables)
		{
			obj.OnDestroyed = (Action)Delegate.Combine(obj.OnDestroyed, new Action(OnDestroyed));
		}
	}

	private void OnDisable()
	{
		Destroyable[] destroyables = _destroyables;
		foreach (Destroyable obj in destroyables)
		{
			obj.OnDestroyed = (Action)Delegate.Remove(obj.OnDestroyed, new Action(OnDestroyed));
		}
	}

	private void OnDestroyed()
	{
		if (!_isConditionMet)
		{
			_countToLose--;
			if (_countToLose == 0)
			{
				OnConditionMet?.Invoke();
				_isConditionMet = true;
			}
		}
	}
}
