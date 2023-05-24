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
        GetComponent<Button>().onClick.AddListener(() => hintArea.SetActive(true));
    }
}
