using System;
using System.Collections;
using LevelEndConditions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LoseScreen : MonoBehaviour
{
	[SerializeField]
	private float _reloadDelay = 3f;

	[SerializeField]
	private GameObject _loseScreen;

	private LevelEndChecker _levelEndChecker;

	[Inject]
	private void Construct(LevelEndChecker levelEndChecker)
	{
		_levelEndChecker = levelEndChecker;
	}

	private void OnEnable()
	{
		LevelEndChecker levelEndChecker = _levelEndChecker;
		levelEndChecker.OnLose = (Action)Delegate.Combine(levelEndChecker.OnLose, new Action(OnLose));
	}

	private void OnDisable()
	{
		LevelEndChecker levelEndChecker = _levelEndChecker;
		levelEndChecker.OnLose = (Action)Delegate.Remove(levelEndChecker.OnLose, new Action(OnLose));
	}

	private void OnLose()
	{
		StartCoroutine(LoseCoroutine());
	}

	private IEnumerator LoseCoroutine()
	{
		yield return new WaitForSeconds(_reloadDelay - 0.5f);
		_loseScreen.SetActive(value: true);
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
