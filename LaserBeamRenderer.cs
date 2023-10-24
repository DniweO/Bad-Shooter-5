using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeamRenderer : MonoBehaviour
{
	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private float _maxDistance = 10f;

	private LineRenderer _lineRenderer;

	private void Start()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, base.transform.right, _maxDistance, _layerMask);
		float num = (raycastHit2D ? raycastHit2D.distance : _maxDistance);
		_lineRenderer.SetPosition(1, Vector3.right * num);
	}
}
