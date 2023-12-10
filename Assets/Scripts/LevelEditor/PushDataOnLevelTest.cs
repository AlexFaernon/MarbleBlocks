using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDataOnLevelTest : MonoBehaviour
{
	private void OnEnable()
	{
		RealtimeDatabase.PushMap(LevelSaveManager.LoadedLevel, true);
		StartCoroutine(RealtimeDatabase.PushRank(AuthManager.User.DisplayName, 0));
	}
}
