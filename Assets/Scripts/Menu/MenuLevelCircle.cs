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
    public int levelNumber;

    public bool IsOpened { get; private set; }
    public bool IsCompleted { get; private set; }

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        LevelSaveManager.LevelNumber = levelNumber;
        SceneManager.LoadScene("Level");
    }
}
