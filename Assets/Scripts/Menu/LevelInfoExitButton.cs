using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoExitButton : MonoBehaviour
{
    [SerializeField] private GameObject levelInfo;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => levelInfo.SetActive(false));
    }
}
