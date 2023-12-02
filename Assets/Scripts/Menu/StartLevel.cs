using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
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
			EnergyManager.SpendEnergy();
		}
		GameMode.CurrentGameMode = GameModeType.SinglePlayer;
		SceneManager.LoadScene("Level");
	}

	private void Update()
	{
		if (LevelSaveManager.LevelNumber <= 3)
		{
			_button.interactable = true;
			return;
		}
		
		_button.interactable = EnergyManager.CurrentEnergy > 0;
	}
}
