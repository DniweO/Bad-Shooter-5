using System;
using LevelEndConditions;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class Pistol : MonoBehaviour
{
	[SerializeField]
	private Transform _bulletSpawnPoint;

	[SerializeField]
	private Bullet _bulletPrefab;

	[SerializeField]
	private ParticleSystem _muzzleFlash;

	[SerializeField]
	private ParticleSystem _noAmmoEffect;

	[SerializeField]
	private FloatingCharacter _floatingCharacter;

	[SerializeField]
	private bool _isInfiniteAmmo;

	[Header("Sounds")]
	[SerializeField]
	private AudioClip _shootSound;

	[SerializeField]
	private AudioClip _noAmmoSound;

	[SerializeField]
	private AudioClip _bulletAddedSound;

	private AudioSource _audioSource;

	private int _bulletsCount;

	private LevelEndChecker _levelEndChecker;

	public Action OnBulletCountChanged;

	public int BulletsCount => _bulletsCount;

	private bool IsNotActive
	{
		get
		{
			if (!_floatingCharacter.IsNotActive)
			{
				return _levelEndChecker.IsLevelEnded;
			}
			return true;
		}
	}

	[Inject]
	private void Construct(LevelEndChecker levelEndChecker)
	{
		_levelEndChecker = levelEndChecker;
	}

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void Shoot()
	{
		if (!IsNotActive)
		{
			if (_bulletsCount <= 0 && !_isInfiniteAmmo)
			{
				NoAmmo();
				return;
			}
			UnityEngine.Object.Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
			_muzzleFlash.Play();
			_audioSource.PlayOneShot(_shootSound);
			_bulletsCount--;
			OnBulletCountChanged?.Invoke();
		}
	}

	public void NoAmmo()
	{
		_noAmmoEffect.Play();
		_audioSource.PlayOneShot(_noAmmoSound);
	}

	public void AddAmmo()
	{
		_bulletsCount++;
		_audioSource.PlayOneShot(_bulletAddedSound);
		OnBulletCountChanged?.Invoke();
	}
}
