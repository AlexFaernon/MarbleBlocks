using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
	private void Awake()
	{
		MenuLevelCircle.LevelCompletion[LevelSaveManager.LevelNumber] = true;
		GetComponent<Button>().onClick.AddListener(NextLevel);
	}

	private void NextLevel()
	{
		LevelSaveManager.LevelNumber++;
		LevelInfo.LoadNextLevel = true;
		SceneManager.LoadScene("MainMenu");
	}
}
