using UnityEngine;
using Zenject;

public class PickupTrigger : MonoBehaviour
{
	private Pistol _pistol;

	[Inject]
	private void Construct(Pistol pistol)
	{
		_pistol = pistol;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent<PickupableAmmo>(out var component))
		{
			component.Pickup(base.transform, delegate
			{
				_pistol.AddAmmo();
			});
		}
	}
}
