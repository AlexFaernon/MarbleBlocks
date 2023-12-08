using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelInEditor : MonoBehaviour
{
	[SerializeField] private Grid grid;
	[SerializeField] private Button button;
	private void Awake()
	{
		StartCoroutine(LoadEditorLevel());
	}

	private void Update()
	{
		button.interactable = LevelSaveManager.LoadLevelTaskCompleted;
	}

	private IEnumerator LoadEditorLevel()
	{
		yield return new WaitUntil(() => LevelSaveManager.LoadLevelTaskCompleted);

		if (LevelSaveManager.LoadedLevel is not null)
		{
			gameObject.SetActive(false);
			grid.gameObject.SetActive(true);
		}
	}

}
