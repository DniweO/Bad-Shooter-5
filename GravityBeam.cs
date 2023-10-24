using System;
using LevelEndConditions;
using UnityEngine;
using Zenject;

public class GravityBeam : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField]
	private Transform gravityBeamStart;

	[SerializeField]
	private ContactFilter2D contactFilter;

	[SerializeField]
	private float maxDistance = 10f;

	[SerializeField]
	private float force = 10f;

	[SerializeField]
	private float minForceMagnitude = 100f;

	[SerializeField]
	private float maxForceMagnitude = 300f;

	[SerializeField]
	private float deadDistance = 0.1f;

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private FloatingCharacter floatingCharacter;

	[SerializeField]
	private float duration = 3f;

	[SerializeField]
	private AnimationCurve forceCurve;

	private Transform gravityBeamEnd;

	private Vector3 mousePosition;

	private Rigidbody2D currentBody;

	private IDraggable currentDraggable;

	private LevelEndChecker levelEndChecker;

	private bool isMouseButtonPressed;

	private bool isMouseButtonReleased;

	private float timer;

	public Action OnStartMovingBody;

	public Action OnBodyReleased;

	public bool HasBody => currentBody != null;

	public bool HasNoBody => !HasBody;

	public Transform GravityBeamStart => gravityBeamStart;

	public Transform GravityBeamEnd => gravityBeamEnd;

	public Vector3 MousePosition => mousePosition;

	public float Timer => timer;

	public float Duration => duration;

	public float ForceRatio => forceCurve.Evaluate(timer / duration);

	[Inject]
	private void Construct(LevelEndChecker levelEndChecker)
	{
		this.levelEndChecker = levelEndChecker;
	}

	private void Update()
	{
		if (!IsNotActive())
		{
			isMouseButtonPressed = Input.GetMouseButtonDown(1);
			isMouseButtonReleased = Input.GetMouseButtonUp(1);
			Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			mousePosition = new Vector2(vector.x, vector.y);
			if (isMouseButtonReleased)
			{
				ReleaseObject();
			}
			else if (isMouseButtonPressed)
			{
				TryGrabObject();
			}
		}
	}

	private void FixedUpdate()
	{
		if (IsNotActive())
		{
			return;
		}
		if (HasNoBody)
		{
			timer = Mathf.Max(timer - Time.fixedDeltaTime, 0f);
		}
		else if (!currentBody.gameObject.activeInHierarchy || !IsRigidbodyValidTarget(currentBody))
		{
			ReleaseObject();
		}
		else
		{
			if (!HasBody)
			{
				return;
			}
			Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			Vector2 vector2 = new Vector2(vector.x, vector.y) - currentBody.position;
			vector2 = ((vector2.magnitude > deadDistance) ? vector2 : Vector2.zero);
			Vector2 vector3 = vector2 * force;
			Collider2D component;
			if (!currentBody.constraints.HasFlag(RigidbodyConstraints2D.FreezeAll))
			{
				timer = Mathf.Min(timer + Time.fixedDeltaTime, duration);
				vector3 = vector3.normalized * Mathf.Lerp(minForceMagnitude, maxForceMagnitude, ForceRatio);
				if (timer >= duration)
				{
					ReleaseObject();
					return;
				}
				currentBody.velocity = vector3;
				if (!currentDraggable.DoRotate)
				{
					currentBody.angularVelocity = 0f;
				}
			}
			else if (currentBody.TryGetComponent<Collider2D>(out component) && component.isTrigger)
			{
				currentBody.transform.position += (Vector3)(vector3 / 5f) * Time.fixedDeltaTime;
			}
		}
	}

	private void TryGrabObject()
	{
		Collider2D[] array = new Collider2D[1];
		if (Physics2D.OverlapPoint(mousePosition, contactFilter, array) == 0)
		{
			return;
		}
		Collider2D collider2D = array[0];
		if (IsPointingAtValidTarget(collider2D))
		{
			IDraggable component = collider2D.GetComponent<IDraggable>();
			if (component != null && component.CanDrag)
			{
				gravityBeamEnd = collider2D.transform;
				currentBody = collider2D.attachedRigidbody;
				currentDraggable = component;
				OnStartMovingBody?.Invoke();
			}
		}
	}

	private void ReleaseObject()
	{
		if (!HasNoBody)
		{
			currentBody = null;
			currentDraggable = null;
			OnBodyReleased?.Invoke();
		}
	}

	private bool IsNotActive()
	{
		if (!floatingCharacter.IsNotActive)
		{
			return levelEndChecker.IsLevelEnded;
		}
		return true;
	}

	private bool IsPointingAtValidTarget(Collider2D hit)
	{
		if (!hit || !hit.attachedRigidbody)
		{
			return false;
		}
		if (hit.attachedRigidbody.bodyType != 0)
		{
			return false;
		}
		return !(Vector2.Distance(gravityBeamStart.position, hit.transform.position) > maxDistance);
	}

	private bool IsRigidbodyValidTarget(Rigidbody2D rigidbody)
	{
		if (!rigidbody || rigidbody.bodyType != 0)
		{
			return false;
		}
		return !(Vector2.Distance(gravityBeamStart.position, rigidbody.transform.position) > maxDistance);
	}
}
