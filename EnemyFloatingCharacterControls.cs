using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyFloatingCharacterControls : FloatingCharacterControls
{
	private FloatingCharacter _bob;

	[Inject]
	private void Construct(FloatingCharacter bob)
	{
		_bob = bob;
	}

	private void Start()
	{
		base.Horizontal = 0f;
		base.IsCrouching = false;
	}

	private void OnEnable()
	{
		FloatingCharacter bob = _bob;
		bob.OnDeath = (Action)Delegate.Combine(bob.OnDeath, new Action(StartCelebrate));
	}

	private void OnDisable()
	{
		FloatingCharacter bob = _bob;
		bob.OnDeath = (Action)Delegate.Remove(bob.OnDeath, new Action(StartCelebrate));
	}

	private void StartCelebrate()
	{
		StartCoroutine(Celebrate());
	}

	private IEnumerator Celebrate()
	{
		while (true)
		{
			float horizontal = Mathf.Sin(Time.time * 20f);
			bool isCrouching = Mathf.Cos(Time.time * 17f) - 0.3f > 0f;
			base.Horizontal = horizontal;
			base.IsCrouching = isCrouching;
			yield return null;
		}
	}
}
