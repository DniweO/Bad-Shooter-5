using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSoundOnStart : MonoBehaviour
{
	[SerializeField]
	private AudioClip[] _clips;

	[SerializeField]
	private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);

	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
		_audioSource.PlayOneShot(_clips[Random.Range(0, _clips.Length)]);
	}
}
