using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LevelEndConditions
{
	public class LevelEndChecker : MonoBehaviour
	{
		[SerializeField]
		private LevelEndCondition[] _conditions;

		[SerializeField]
		private LevelEndCondition[] _loseConditions;

		private int _metConditionsCount;

		private int _metLoseConditionsCount;

		private FloatingCharacter _bob;

		private bool _isLost;

		public Action OnLevelEnd;

		public Action OnLose;

		public bool IsLevelEnded { get; private set; }

		[Inject]
		private void Construct(FloatingCharacter bob)
		{
			_bob = bob;
		}

		private void OnEnable()
		{
			LevelEndCondition[] conditions = _conditions;
			foreach (LevelEndCondition obj in conditions)
			{
				obj.OnConditionMet = (Action)Delegate.Combine(obj.OnConditionMet, new Action(OnConditionMet));
			}
			conditions = _loseConditions;
			foreach (LevelEndCondition obj2 in conditions)
			{
				obj2.OnConditionMet = (Action)Delegate.Combine(obj2.OnConditionMet, new Action(OnLoseConditionMet));
			}
			FloatingCharacter bob = _bob;
			bob.OnDeath = (Action)Delegate.Combine(bob.OnDeath, new Action(OnBobDeath));
		}

		private void OnDisable()
		{
			LevelEndCondition[] conditions = _conditions;
			foreach (LevelEndCondition obj in conditions)
			{
				obj.OnConditionMet = (Action)Delegate.Remove(obj.OnConditionMet, new Action(OnConditionMet));
			}
			conditions = _loseConditions;
			foreach (LevelEndCondition obj2 in conditions)
			{
				obj2.OnConditionMet = (Action)Delegate.Remove(obj2.OnConditionMet, new Action(OnLoseConditionMet));
			}
			FloatingCharacter bob = _bob;
			bob.OnDeath = (Action)Delegate.Remove(bob.OnDeath, new Action(OnBobDeath));
		}

		private void OnConditionMet()
		{
			_metConditionsCount++;
			if (!_isLost)
			{
				PlayerPrefs.SetInt("Current Level", SceneManager.GetActiveScene().buildIndex);
				PlayerPrefs.Save();
				if (_metConditionsCount == _conditions.Length)
				{
					StartCoroutine(WinCoroutine());
				}
			}
		}

		private void OnLoseConditionMet()
		{
			_metLoseConditionsCount++;
			if (_metLoseConditionsCount == _loseConditions.Length)
			{
				_bob.IsActive = false;
				OnBobDeath();
			}
		}

		private void OnBobDeath()
		{
			_isLost = true;
			OnLose?.Invoke();
		}

		private IEnumerator WinCoroutine()
		{
			yield return new WaitForSeconds(0.5f);
			if (!_isLost)
			{
				IsLevelEnded = true;
				OnLevelEnd?.Invoke();
			}
		}
	}
}
