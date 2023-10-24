using System;
using UnityEngine;

namespace Mechanisms
{
	public abstract class Activator : MonoBehaviour
	{
		public Action OnActivated;

		public Action OnDeactivated;
	}
}
