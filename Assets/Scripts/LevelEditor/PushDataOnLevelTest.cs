using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDataOnLevelTest : MonoBehaviour
{
	private void OnEnable()
	{
		RealtimeDatabase.PushMap(LevelSaveManager.LoadedLevel, true);
		RealtimeDatabase.PushRank();
	}
}
