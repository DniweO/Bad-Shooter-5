using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceMaterial", menuName = "ScriptableObjects/SurfaceMaterial", order = 1)]
public class SurfaceMaterial : ScriptableObject
{
	public ParticleSystem hitEffect;

	public ParticleSystem destroyEffect;

	public int bulletDamage = 1;
}
