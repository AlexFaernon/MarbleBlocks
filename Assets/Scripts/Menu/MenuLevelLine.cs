using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevelLine : MonoBehaviour
{
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite completed;
    [SerializeField] private MenuLevelCircle nextLevelCircle;

    private void Awake()
    {
        if (nextLevelCircle.IsCompleted)
        {
            GetComponent<Image>().sprite = completed;
        }
        else if (nextLevelCircle.IsOpened)
        {
            GetComponent<Image>().sprite = opened;
        }
        else
        {
            GetComponent<Image>().sprite = closed;
        }
    }
}
