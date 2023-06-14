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
    [SerializeField] private List<Sprite> area3Backgrounds;


    private void Awake()
    {
        var random = new Random();
        GetComponent<Image>().sprite = LevelSaveManager.LevelNumber switch
        {
            <= 9 => area1Backgrounds[random.Next(area1Backgrounds.Count)],
            > 9 and < 17 => area2Backgrounds[random.Next(area2Backgrounds.Count)],
            >= 17 => area3Backgrounds[random.Next(area3Backgrounds.Count)]
        };
    }
}
