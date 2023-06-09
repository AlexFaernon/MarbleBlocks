using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    [SerializeField] private GameObject hintArea;
    private void Awake()
    {
        if (LevelSaveManager.LevelNumber < 4)
        {
            gameObject.SetActive(false);
            return;
        }
        GetComponent<Button>().onClick.AddListener(() => hintArea.SetActive(true));
    }
}
