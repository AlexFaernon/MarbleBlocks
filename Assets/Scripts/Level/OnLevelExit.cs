using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLevelExit : MonoBehaviour
{
	private void OnDestroy()
	{
		if (GameMode.CurrentGameMode == GameModeType.MultiPlayer && !WinLoseManager.WinMultiplayer.activeSelf)
		{
			StartCoroutine(RealtimeDatabase.PushRank(RealtimeDatabase.Opponent, 3));
		}
	}
}
