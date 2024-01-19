using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    [SerializeField] private GameObject hintArea;
    [FormerlySerializedAs("helpManager")]
    [SerializeField] private HelpSwitch helpSwitch;

    [SerializeField] private TMP_Text helpCount;
    private Button _button;
    private void Awake()
    {
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer && LevelSaveManager.LevelNumber < 4)
        {
            gameObject.SetActive(false);
            return;
        }
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => hintArea.SetActive(true));
    }

    private void Update()
    {
        _button.interactable = helpSwitch.HelpLevel < helpSwitch.MaxHelpLevels;
        helpCount.text = (helpSwitch.MaxHelpLevels - helpSwitch.HelpLevel).ToString();
    }
}
