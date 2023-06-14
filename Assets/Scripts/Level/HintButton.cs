using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    [SerializeField] private GameObject hintArea;
    [SerializeField] private HelpManager helpManager;
    private Button _button;
    private void Awake()
    {
        if (LevelSaveManager.LevelNumber < 4)
        {
            gameObject.SetActive(false);
            return;
        }
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => hintArea.SetActive(true));
    }

    private void Update()
    {
        _button.interactable = helpManager.HelpLevel < 3;
    }
}
