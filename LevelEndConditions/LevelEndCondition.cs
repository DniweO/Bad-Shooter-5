using System;
using UnityEngine;

namespace LevelEndConditions
{
	public abstract class LevelEndCondition : MonoBehaviour
	{
		public abstract Action OnConditionMet { get; set; }
	}
}
