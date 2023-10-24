using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(GravityBeam))]
	public class GravityBeamView : MonoBehaviour
	{
		[SerializeField]
		private LineRenderer _lineRenderer;

		[SerializeField]
		private int _segments = 20;

		[SerializeField]
		private float _controlPointDistance = 50f;

		[SerializeField]
		private TextMeshProUGUI _debugText;

		private GravityBeam _gravityBeam;

		private Vector2 _controlPoint;

		private BezierCurve _bezierCurve;

		private Transform GravityBeamStart => _gravityBeam.GravityBeamStart;

		private Transform GravityBeamEnd => _gravityBeam.GravityBeamEnd;

		private Vector3 MousePosition => _gravityBeam.MousePosition;

		private void Awake()
		{
			_gravityBeam = GetComponent<GravityBeam>();
		}

		private void OnEnable()
		{
			GravityBeam gravityBeam = _gravityBeam;
			gravityBeam.OnStartMovingBody = (Action)Delegate.Combine(gravityBeam.OnStartMovingBody, new Action(OnStartMovingBody));
			GravityBeam gravityBeam2 = _gravityBeam;
			gravityBeam2.OnBodyReleased = (Action)Delegate.Combine(gravityBeam2.OnBodyReleased, new Action(OnBodyReleased));
		}

		private void OnDisable()
		{
			GravityBeam gravityBeam = _gravityBeam;
			gravityBeam.OnStartMovingBody = (Action)Delegate.Remove(gravityBeam.OnStartMovingBody, new Action(OnStartMovingBody));
			GravityBeam gravityBeam2 = _gravityBeam;
			gravityBeam2.OnBodyReleased = (Action)Delegate.Remove(gravityBeam2.OnBodyReleased, new Action(OnBodyReleased));
		}

		private void OnStartMovingBody()
		{
			_lineRenderer.enabled = true;
			_lineRenderer.positionCount = _segments;
		}

		private void OnBodyReleased()
		{
			_lineRenderer.positionCount = 0;
			_lineRenderer.enabled = false;
		}

		private void Update()
		{
			_debugText.text = $"Time: {_gravityBeam.Duration - _gravityBeam.Timer:0.00}/{_gravityBeam.Duration:0.00}\nForce ratio: {_gravityBeam.ForceRatio:0.00}";
			if (!_gravityBeam.HasNoBody)
			{
				DrawBeam();
			}
		}

		private void DrawBeam()
		{
			_controlPoint = Vector2.MoveTowards(_controlPoint, MousePosition, Time.fixedDeltaTime * _controlPointDistance);
			_bezierCurve = new BezierCurve
			{
				StartPoint = GravityBeamStart.position,
				EndPoint = GravityBeamEnd.position,
				ControlPoint = _controlPoint
			};
			_lineRenderer.positionCount = _segments;
			for (int i = 0; i < _segments; i++)
			{
				float t = (float)i / (float)_segments;
				Vector2 point = _bezierCurve.GetPoint(t);
				_lineRenderer.SetPosition(i, point);
			}
		}
	}
}
