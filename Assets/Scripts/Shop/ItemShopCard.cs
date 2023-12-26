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
    private ref int _limit;

    private void Awake()
    {
        costLabel.text = cost.ToString();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void Update()
    {
        buyButton.interactable = cost <= CoinsManager.Coins;
    }

    private void BuyItem()
    {
        CoinsManager.Coins -= cost;
        switch (objectType)
        {
            case OnTileObject.None:
                throw new ArgumentException("bruuuh buying nothing");
            case OnTileObject.Spike:
                LevelObjectsLimits.Spike++;
                break;
            case OnTileObject.Whirlpool:
                LevelObjectsLimits.Whirlpool++;
                break;
            case OnTileObject.Lever:
                LevelObjectsLimits.Lever++;
                break;
            case OnTileObject.WaterLily:
                LevelObjectsLimits.WaterLily++;
                break;
            case OnTileObject.Teleport:
                LevelObjectsLimits.Teleport++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
