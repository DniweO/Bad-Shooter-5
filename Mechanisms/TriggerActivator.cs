using UnityEngine;

namespace Mechanisms
{
	public class TriggerActivator : Activator
	{
		[SerializeField]
		private LayerMask _layerMask;

		private bool _isActivated;

		private GameObject _activatedBy;

		private void Start()
		{
			OnDeactivated?.Invoke();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!_isActivated && !other.isTrigger && (bool)other.attachedRigidbody && IsInLayerMask(other.gameObject.layer, _layerMask))
			{
				OnActivated?.Invoke();
				_isActivated = true;
				_activatedBy = other.gameObject;
				Debug.Log("Trigger has been activated");
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.Equals(_activatedBy) && _isActivated)
			{
				_isActivated = false;
				_activatedBy = null;
				OnDeactivated?.Invoke();
				Debug.Log("Trigger has been deactivated");
			}
		}

		private bool IsInLayerMask(int layer, LayerMask layerMask)
		{
			return (int)layerMask == ((int)layerMask | (1 << layer));
		}
	}
}
