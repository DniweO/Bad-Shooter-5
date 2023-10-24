using System;
using LevelEndConditions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class WinScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject _winScreen;

	[SerializeField]
	private Button _nextLevelButton;

	private LevelEndChecker _levelEndChecker;

	[Inject]
	private void Construct(LevelEndChecker levelEndChecker)
	{
		_levelEndChecker = levelEndChecker;
		_nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
	}

	private void OnEnable()
	{
		LevelEndChecker levelEndChecker = _levelEndChecker;
		levelEndChecker.OnLevelEnd = (Action)Delegate.Combine(levelEndChecker.OnLevelEnd, new Action(OnLevelEnd));
	}

	private void OnDisable()
	{
		LevelEndChecker levelEndChecker = _levelEndChecker;
		levelEndChecker.OnLevelEnd = (Action)Delegate.Remove(levelEndChecker.OnLevelEnd, new Action(OnLevelEnd));
	}

	private void OnLevelEnd()
	{
		_winScreen.SetActive(value: true);
	}

	private void OnNextLevelButtonClicked()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
