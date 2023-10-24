using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestMainMenu : MonoBehaviour
{
	[SerializeField]
	private Button _startButton;

	private void Start()
	{
		_startButton.onClick.AddListener(delegate
		{
			SceneManager.LoadScene(PlayerPrefs.GetInt("Current Level", 1));
		});
	}
}
