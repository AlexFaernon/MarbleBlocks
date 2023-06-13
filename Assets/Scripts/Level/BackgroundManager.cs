using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> area1Backgrounds;
    [SerializeField] private List<Sprite> area2Backgrounds;

    private void Awake()
    {
        var random = new Random();
        GetComponent<Image>().sprite = LevelSaveManager.LevelNumber switch
        {
            <=11 => area1Backgrounds[random.Next(area1Backgrounds.Count)],
            >11 => area2Backgrounds[random.Next(area2Backgrounds.Count)],
        };
    }
}
