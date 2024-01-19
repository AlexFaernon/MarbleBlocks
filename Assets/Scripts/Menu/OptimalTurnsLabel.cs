using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptimalTurnsLabel : MonoBehaviour
{
    private TMP_Text _label;

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _label.text = LevelSaveManager.LoadedLevel.OptimalTurns.ToString();
    }
}
