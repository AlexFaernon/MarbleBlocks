using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private GameObject levelInfo;
    public static bool LoadNextLevel;

    private void Awake()
    {
        if (LoadNextLevel)
        {
            levelInfo.SetActive(true);
            LoadNextLevel = false;
        }
    }
}
