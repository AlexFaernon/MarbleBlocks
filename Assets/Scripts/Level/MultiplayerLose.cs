using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerLose : MonoBehaviour
{
	private void Awake()
	{
		StartCoroutine(RealtimeDatabase.PushRank(RealtimeDatabase.Opponent, 1));
		RealtimeDatabase.PushToHistory(RealtimeDatabase.Opponent, StepCounter.Count, false);
	}
}
