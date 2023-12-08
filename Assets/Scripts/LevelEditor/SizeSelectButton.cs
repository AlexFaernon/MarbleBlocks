using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeSelectButton : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    private bool _isPreviouslyEnabled;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (_isPreviouslyEnabled)
        { 
            Debug.Log("can remake");
            _button.onClick.RemoveListener(RemakeLevel); 
            _button.onClick.AddListener(RemakeLevel);
        }
        else
        {
            Debug.Log("cannot remake");
            _isPreviouslyEnabled = true;
        }
    }

    private void RemakeLevel()
    {
        tileManager.RemakeLevelInEditor();
    }
}
