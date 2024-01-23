using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTutorialHint : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerData.SingleLevelCompleted < 4 || PlayerPrefs.HasKey("OpenedEditor"))
		{
			gameObject.SetActive(false);
		}
	}
}
