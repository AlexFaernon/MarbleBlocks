using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLeaderboard : MonoBehaviour
{
	private Button _button;
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(() => StartCoroutine(Load()));
		_button.interactable = false;
	}

	private IEnumerator Load()
	{
		StartCoroutine(RealtimeDatabase.ExportLeaderboard());
		yield return new WaitUntil(() => RealtimeDatabase.LeaderboardLoaded);

		SceneManager.LoadScene("Leaderboard");
	}
}