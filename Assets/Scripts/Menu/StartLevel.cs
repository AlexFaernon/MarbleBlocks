using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
	[SerializeField] private EnergyManager energyManager;
	[SerializeField] private TMP_Text label;
	private Button _button;
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(LoadLevel);
	}

	private void LoadLevel()
	{
		if (LevelSaveManager.LevelNumber > 3)
		{
			energyManager.SpendEnergy();
		}
		SceneManager.LoadScene("Level");
	}

	private void Update()
	{
		if (LevelSaveManager.LevelNumber <= 3)
		{
			_button.interactable = true;
			label.text = "Начать уровень";
			return;
		}
		
		_button.interactable = EnergyManager.CurrentEnergy > 0;
		label.text = EnergyManager.CurrentEnergy > 0 ? "Начать уровень" : "Не хватает энергии";
	}
}
