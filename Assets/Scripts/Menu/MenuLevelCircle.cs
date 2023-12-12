using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevelCircle : MonoBehaviour
{
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite completed;
    [SerializeField] private GameObject levelPreview;
    [SerializeField] private TMP_Text numberLabel;
    public int levelNumber;

    private Button _button;
    private Image _image;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        numberLabel.text = levelNumber.ToString();

        _button.onClick.AddListener(LoadLevel);
    }

    private void Update()
    {
        if (PlayerData.SingleLevelCompleted >= levelNumber)
        {
            _image.sprite = completed;
            _button.interactable = true;
        }
        else if (levelNumber - 1 == PlayerData.SingleLevelCompleted)
        {
            _image.sprite = opened;
            _button.interactable = true;
        }
        else
        {
            _image.sprite = closed;
            _button.interactable = false;
        }
    }

    // private void Update()
    // {
    //     if (PlayerData.Name.ToUpper() == "TRW")
    //     {
    //         _image.sprite = completed;
    //         _button.interactable = true;
    //     }
    // }

    private void LoadLevel()
    {
        LevelSaveManager.LevelNumber = levelNumber;
        levelPreview.SetActive(true);
    }
}
