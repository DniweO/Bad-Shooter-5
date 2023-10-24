using System;
using UnityEngine;

namespace LevelEndConditions
{
	public class OnFloatingCharacterDeathLevelEndCondition : LevelEndCondition
	{
		[SerializeField]
		private FloatingCharacter _floatingCharacter;

		private bool _isConditionMet;

		public override Action OnConditionMet { get; set; }

		private void OnEnable()
		{
			FloatingCharacter floatingCharacter = _floatingCharacter;
			floatingCharacter.OnDeath = (Action)Delegate.Combine(floatingCharacter.OnDeath, new Action(OnFloatingCharacterDeath));
		}

		private void OnDisable()
		{
			FloatingCharacter floatingCharacter = _floatingCharacter;
			floatingCharacter.OnDeath = (Action)Delegate.Remove(floatingCharacter.OnDeath, new Action(OnFloatingCharacterDeath));
		}

		private void OnFloatingCharacterDeath()
		{
			if (!_isConditionMet)
			{
				_isConditionMet = true;
				OnConditionMet?.Invoke();
			}
		}
	}
}
