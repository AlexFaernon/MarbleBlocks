using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuLevelCircle : MonoBehaviour
{
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite completed;
    [SerializeField] private MenuLevelCircle previousLevelCircle;
    [SerializeField] private GameObject levelPreview;
    [SerializeField] private TMP_Text numberLabel;
    public int levelNumber;

    private Button _button;
    private Image _image;
    public bool isCompleted;
    public bool isOpened;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        numberLabel.text = levelNumber.ToString();

        _button.onClick.AddListener(LoadLevel);
        if (levelNumber == 1)
        {
            isOpened = true;
        }
        else if (PlayerPrefs.HasKey($"level{levelNumber - 1}"))
        {
            isOpened = Convert.ToBoolean(PlayerPrefs.GetInt($"level{levelNumber - 1}"));
        }
        
        if (PlayerPrefs.HasKey($"level{levelNumber}"))
        {
            isCompleted = Convert.ToBoolean(PlayerPrefs.GetInt($"level{levelNumber}"));
        }

        if (isCompleted)
        {
            _image.sprite = completed;
        }
        else if (isOpened)
        {
            _image.sprite = opened;
        }
        else
        {
            _image.sprite = closed;
            _button.interactable = false;
        }
    }

    private void Update()
    {
        if (NameManager.PlayerName.ToUpper() == "TRW")
        {
            _image.sprite = completed;
            _button.interactable = true;
        }
    }

    private void LoadLevel()
    {
        LevelSaveManager.LevelNumber = levelNumber;
        levelPreview.SetActive(true);
    }
}
