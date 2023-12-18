using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMultiplayerLevel : MonoBehaviour
{
	[SerializeField] private List<GameObject> gameUI;
	[SerializeField] private List<GameObject> levelSelectUI;
	private Button _button;
	private void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(StartLevel);
		foreach (var o in gameUI)
		{
			o.SetActive(GameMode.CurrentGameMode == GameModeType.SinglePlayer);
		}
		
		foreach (var o in levelSelectUI)
		{
			o.SetActive(GameMode.CurrentGameMode == GameModeType.MultiPlayer);
		}
	}

	private void StartLevel()
	{
		foreach (var o in gameUI)
		{
			o.SetActive(true);
		}
		
		foreach (var o in levelSelectUI)
		{
			o.SetActive(false);
		}
		
		if (LevelSaveManager.LevelNumber > 3)
		{
			EnergyManager.SpendEnergy();
		}
	}

	private void Update()
	{
		_button.interactable = EnergyManager.CurrentEnergy > 0 && LevelSaveManager.LoadLevelTaskCompleted;
	}
}
