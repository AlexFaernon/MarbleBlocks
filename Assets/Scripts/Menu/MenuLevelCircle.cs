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

    public static bool[] LevelCompletion = new bool[20];
    private Button _button;
    private void Awake()
    {
        LevelCompletion[0] = true;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadLevel);
        
        if (!LevelCompletion[levelNumber])
        {
            if (!LevelCompletion[levelNumber - 1])
            {
                _button.interactable = false;
                GetComponent<Image>().sprite = closed;
            }
            else
            {
                IsOpened = true;
                GetComponent<Image>().sprite = opened;
            }
        }
        else
        {
            IsCompleted = true;
        }

        StartCoroutine(WaitUntilName());
    }

    private IEnumerator WaitUntilName()
    {
        yield return new WaitUntil(() => NameManager.PlayerName != "");

        if (NameManager.PlayerName == "TRW")
        {
            GetComponent<Image>().sprite = completed;
            _button.interactable = true;
        }
    }

    private void LoadLevel()
    {
        LevelSaveManager.LevelNumber = levelNumber;
        levelPreview.SetActive(true);
    }
}
