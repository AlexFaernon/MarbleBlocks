using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerTutorial : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerData.SingleLevelCompleted >= 4 && PlayerPrefs.HasKey("OpenedEditor") && !PlayerPrefs.HasKey("MultiplayerTutorial"))
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(true);
			}

			PlayerPrefs.SetInt("MultiplayerTutorial", 1);
		}
	}
}
