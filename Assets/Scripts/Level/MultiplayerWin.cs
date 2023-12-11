using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerWin : MonoBehaviour
{
	private void Awake()
	{
		StartCoroutine(RealtimeDatabase.PushRank(AuthManager.User.DisplayName, 3));
		StartCoroutine(RealtimeDatabase.PushRank(RealtimeDatabase.Opponent, -1));
		StartCoroutine(RealtimeDatabase.IncreaseLevelCount());
		RealtimeDatabase.PushToHistory(RealtimeDatabase.Opponent, StepCounter.Count, true);

	}
}
