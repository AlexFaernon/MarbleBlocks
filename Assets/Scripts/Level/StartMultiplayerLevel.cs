using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMultiplayerLevel : MonoBehaviour
{
	[SerializeField] private List<GameObject> gameUI;
	[SerializeField] private List<GameObject> levelSelectUI;
	private Button _button;
	private LeanDragCamera _dragCamera;
	private Physics2DRaycaster _raycaster;
	private void Awake()
	{
		_dragCamera = Camera.main.GetComponent<LeanDragCamera>();
		_raycaster = Camera.main.GetComponent<Physics2DRaycaster>();
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

		if (GameMode.CurrentGameMode == GameModeType.MultiPlayer)
		{
			_dragCamera.enabled = false;
			_raycaster.enabled = false;
		}
	}

	private void StartLevel()
	{
		_dragCamera.enabled = true;
		_raycaster.enabled = true;
		
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
