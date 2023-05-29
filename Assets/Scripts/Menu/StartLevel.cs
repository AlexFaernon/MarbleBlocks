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
		energyManager.SpendEnergy();
		SceneManager.LoadScene("Level");
	}

	private void Update()
	{
		_button.interactable = EnergyManager.CurrentEnergy > 0;
		label.text = EnergyManager.CurrentEnergy > 0 ? "Начать уровень" : "Не хватает энергии";
	}
}
