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
                case DoorLeverColor.Blue:
                    return ref LevelObjectsLimits.Blue;
                case DoorLeverColor.Red:
                case DoorLeverColor.Grey:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private int LevelRequirement
    {
        get
        {
            return color switch
            {
                DoorLeverColor.Red => 0,
                DoorLeverColor.Grey => 0,
                DoorLeverColor.Blue => LevelObjectsLimits.BlueLevel,
                DoorLeverColor.Purple => LevelObjectsLimits.PurpleLevel,
                DoorLeverColor.Green => LevelObjectsLimits.GreenLevel,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    
    private void Awake()
    {
        buyButton.onClick.AddListener(BuyItem);
        if (PlayerData.SingleLevelCompleted >= LevelRequirement) return;
        
        lockImage.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        lockImage.GetComponentInChildren<TMP_Text>().text = $"{LevelRequirement} уровень";
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
