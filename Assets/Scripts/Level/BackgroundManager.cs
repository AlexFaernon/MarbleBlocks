using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgrounds;

    private void Awake()
    {
        var random = new Random();
        GetComponent<Image>().sprite = backgrounds[random.Next(backgrounds.Count)];
    }
}
