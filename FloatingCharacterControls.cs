using UnityEngine;

public abstract class FloatingCharacterControls : MonoBehaviour
{
	public float Horizontal { get; protected set; }

	public bool IsCrouching { get; protected set; }
}
