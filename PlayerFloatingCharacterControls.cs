using UnityEngine;

public class PlayerFloatingCharacterControls : FloatingCharacterControls
{
	private void Update()
	{
		base.Horizontal = Input.GetAxisRaw("Horizontal");
		base.IsCrouching = Input.GetKey(KeyCode.S);
	}
}
