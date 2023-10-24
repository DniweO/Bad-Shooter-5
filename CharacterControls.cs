using UnityEngine;

public abstract class CharacterControls : MonoBehaviour
{
	[SerializeField]
	private AimingController aimingController;

	[SerializeField]
	private Transform hips;

	[SerializeField]
	private Pistol pistol;

	[SerializeField]
	private Transform aimTarget;

	protected void SetAimingPosition(Vector3 position)
	{
		aimTarget.position = position;
	}

	protected void StartAiming()
	{
		aimingController.isAiming = true;
	}

	protected void CancelAiming()
	{
		aimingController.isAiming = false;
	}

	protected void Shoot()
	{
		aimingController.isAiming = false;
		pistol.Shoot();
	}

	protected void ChooseHand()
	{
		Vector3 position = aimTarget.position;
		if (aimingController.isAiming)
		{
			Wrists wrists = ((!(position.x > hips.position.x)) ? Wrists.Left : Wrists.Right);
			if (aimingController.currentWrist != wrists)
			{
				aimingController.SetRulingHand(wrists);
			}
		}
	}
}
