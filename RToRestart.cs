using UnityEngine;
using Zenject;

public class RToRestart : MonoBehaviour
{
	private FloatingCharacter _bob;

	[Inject]
	private void Construct(FloatingCharacter bob)
	{
		_bob = bob;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			_bob.IsActive = false;
		}
	}
}
