using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopCard : MonoBehaviour
{
    [SerializeField] private Button buyButton;
    [SerializeField] private OnTileObject objectType;
    [SerializeField] private int maxCount;
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costLabel;
    [SerializeField] private Image lockImage;
    [SerializeField] private TMP_Text count;
    
    private void Awake()
    {
        costLabel.text = cost.ToString();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void Update()
    {
        buyButton.interactable = cost <= CoinsManager.Coins && ObjectLimit() < maxCount;
        count.text = $"{ObjectLimit()}/{maxCount}";
    }

    private void BuyItem()
    {
        CoinsManager.Coins -= cost;
        ObjectLimit(+1);
        RealtimeDatabase.PushShop();
    }

    private int ObjectLimit(int delta = 0)
    {
        switch (objectType)
        {
            case OnTileObject.None:
                throw new ArgumentException("bruuuh buying nothing");
            case OnTileObject.Spike:
                LevelObjectsLimits.Spike += delta;
                return LevelObjectsLimits.Spike;
            case OnTileObject.Whirlpool:
                LevelObjectsLimits.Whirlpool += delta;
                return LevelObjectsLimits.Whirlpool;
            case OnTileObject.Lever:
                LevelObjectsLimits.Lever += delta;
                return LevelObjectsLimits.Lever;
            case OnTileObject.WaterLily:
                LevelObjectsLimits.WaterLily += delta;
                return LevelObjectsLimits.WaterLily;
            case OnTileObject.Teleport:
                LevelObjectsLimits.Teleport += delta;
                return LevelObjectsLimits.Teleport;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
