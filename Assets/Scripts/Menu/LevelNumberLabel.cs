using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelNumberLabel : MonoBehaviour
{
	private TMP_Text label;

	private void Awake()
	{
		label = GetComponent<TMP_Text>();
	}
	
	private void Update()
	{
		label.text = $"{LevelSaveManager.LevelNumber} Уровень";
	}
}
