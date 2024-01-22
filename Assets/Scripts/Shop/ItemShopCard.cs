using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopCard : MonoBehaviour
{
    [SerializeField] private Button buyButton;
    [SerializeField] private LimitedObject objectType;
    [SerializeField] private int maxCount;
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text costLabel;
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private TMP_Text count;
    private ref int ObjectLimit
    {
        get
        {
            switch (objectType)
            {
                case LimitedObject.Spike:
                    return ref LevelObjectsLimits.Spike;
                case LimitedObject.Whirlpool:
                    return ref LevelObjectsLimits.Whirlpool;
                case LimitedObject.Lever:
                    return ref LevelObjectsLimits.Lever;
                case LimitedObject.WaterLily:
                    return ref LevelObjectsLimits.WaterLily;
                case LimitedObject.Teleport:
                    return ref LevelObjectsLimits.Teleport;
                case LimitedObject.Gate:
                    return ref LevelObjectsLimits.Gate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private int LevelRequirement
    {
        get
        {
            return objectType switch
            {
                LimitedObject.Spike => 0,
                LimitedObject.Whirlpool => 0,
                LimitedObject.Lever => LevelObjectsLimits.LeverLevel,
                LimitedObject.WaterLily => LevelObjectsLimits.WaterlilyLevel,
                LimitedObject.Teleport => LevelObjectsLimits.TeleportLevel,
                LimitedObject.Gate => LevelObjectsLimits.GateLevel,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    private void Awake()
    {
        costLabel.text = cost.ToString();
        buyButton.onClick.AddListener(BuyItem);
        if (PlayerData.SingleLevelCompleted >= LevelRequirement) return;

        lockImage.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        count.transform.parent.gameObject.SetActive(false);
        lockImage.GetComponentInChildren<TMP_Text>().text = $"{LevelRequirement} уровень";
        GetComponent<Image>().sprite = lockSprite;
    }

    private void Update()
    {
        buyButton.interactable = cost <= CoinsManager.Coins && ObjectLimit < maxCount;
        count.text = $"{ObjectLimit}/{maxCount}";
        if (!buyButton.interactable)
        {
            costLabel.text = "Куплено";
        }
    }

    private void BuyItem()
    {
        CoinsManager.Coins -= cost;
        ObjectLimit += 1;
        RealtimeDatabase.PushShop();
    }
}
