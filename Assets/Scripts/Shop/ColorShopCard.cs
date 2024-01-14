using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ColorShopCard : MonoBehaviour
{
    [SerializeField] private Button buyButton;
    [SerializeField] private DoorLeverColor color;
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costLabel;
    [SerializeField] private Image lockImage;
    private ref bool GetColorField
    {
        get
        {
            switch (color)
            {
                case DoorLeverColor.Purple:
                    return ref LevelObjectsLimits.Purple;
                case DoorLeverColor.Green:
                    return ref LevelObjectsLimits.Green;
                case DoorLeverColor.Yellow:
                    return ref LevelObjectsLimits.Yellow;
                case DoorLeverColor.Red:
                case DoorLeverColor.Grey:
                case DoorLeverColor.Blue:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private void Awake()
    {
        buyButton.onClick.AddListener(BuyItem);
    }

    private void Update()
    {
        buyButton.interactable = !GetColorField;
        costLabel.text = GetColorField ? "Куплено" : cost.ToString();
    }

    private void BuyItem()
    {
        CoinsManager.Coins -= cost;
        GetColorField = true;
        RealtimeDatabase.PushShop();
    }
}
