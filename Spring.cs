using System;
using UnityEngine;

[Serializable]
public class Spring
{
	public float value;

	public float velocity;

	public float target;

	public float damping;

	public float frequency;

	public bool isFinished;

	private float _epsilon = 0.001f;

	public Spring(float frequency, float damping)
	{
		this.damping = damping;
		this.frequency = frequency;
	}

	public Spring(float value, float velocity, float target, float damping, float frequency)
	{
		this.value = value;
		this.velocity = velocity;
		this.target = target;
		this.damping = damping;
		this.frequency = frequency;
	}

	public void Update(float deltaTime)
	{
		if (Mathf.Abs(velocity) < _epsilon && Mathf.Abs(target - value) < _epsilon)
		{
			velocity = 0f;
			isFinished = true;
			return;
		}
		isFinished = false;
		float num = frequency * 2f * MathF.PI;
		float num2 = 1f + 2f * deltaTime * damping * num;
		float num3 = num * num;
		float num4 = deltaTime * num3;
		float num5 = deltaTime * num4;
		float num6 = 1f / (num2 + num5);
		float num7 = num2 * value + deltaTime * velocity + num5 * target;
		float num8 = velocity + num4 * (target - value);
		value = num7 * num6;
		velocity = num8 * num6;
	}
}
