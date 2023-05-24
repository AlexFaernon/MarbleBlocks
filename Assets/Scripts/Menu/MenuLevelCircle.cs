using System;
using System.Collections;
using System.Collections.Generic;
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
    public int levelNumber;

    public bool IsOpened { get; private set; }
    public bool IsCompleted { get; private set; }

    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        LevelSaveManager.LevelNumber = levelNumber;
        levelPreview.SetActive(true);
    }
}
