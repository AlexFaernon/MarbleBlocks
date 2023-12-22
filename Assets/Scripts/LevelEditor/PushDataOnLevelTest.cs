using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushDataOnLevelTest : MonoBehaviour
{
	[SerializeField] private GameObject window;
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(PushLevel);
	}

	private void PushLevel()
	{
		LevelSaveManager.LoadedLevel.OptimalTurns = StepCounter.Count;
		LevelSaveManager.LoadedLevel.HelpClass = WriteHelpInEditor.GetHelp();
		RealtimeDatabase.PushMap(LevelSaveManager.LoadedLevel, true);
		StartCoroutine(RealtimeDatabase.PushRank(AuthManager.User.DisplayName, 0));
		window.SetActive(false);
	}
}
