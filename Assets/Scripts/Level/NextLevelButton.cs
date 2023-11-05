using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(NextLevel);
	}

	private void NextLevel()
	{
		LevelSaveManager.LevelNumber++;
		LevelInfo.LoadNextLevel = true;
		SceneManager.LoadScene("MainMenu");
	}
}
