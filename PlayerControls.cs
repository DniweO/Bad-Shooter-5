using UnityEngine;

public class PlayerControls : CharacterControls
{
	[SerializeField]
	private Camera _camera;

	private Vector3 MousePosition => _camera.ScreenToWorldPoint(Input.mousePosition);

	private void Update()
	{
		SetAimingPosition(MousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			StartAiming();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			Shoot();
		}
		ChooseHand();
	}
}
