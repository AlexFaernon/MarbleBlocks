using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitEditor : MonoBehaviour
{
	[SerializeField] private LevelSaveManager levelSaveManager;
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() =>
			{
				levelSaveManager.SaveLevel();
				SceneManager.LoadScene("MainMenu");
			});
	}
}
