using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    [SerializeField] private GameObject hintArea;
    [FormerlySerializedAs("helpManager")]
    [SerializeField] private HelpSwitch helpSwitch;
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
        _button.interactable = helpSwitch.HelpLevel < 3;
    }
}
