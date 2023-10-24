using System.Collections;
using UnityEngine;

namespace Mechanisms
{
	public class TimerActivator : Activator
	{
		[SerializeField]
		private float _delay = 10f;

		[SerializeField]
		private float _activeTime = 1.5f;

		private IEnumerator Start()
		{
			while (true)
			{
				yield return new WaitForSeconds(_delay);
				OnActivated?.Invoke();
				yield return new WaitForSeconds(_activeTime);
				OnDeactivated?.Invoke();
			}
		}
	}
}
