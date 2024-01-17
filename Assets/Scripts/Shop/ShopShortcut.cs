using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShortcut : MonoBehaviour
{
    [SerializeField] private GameObject levelObjects;
    [SerializeField] private GameObject coinsObject;
    [SerializeField] private GameObject energyObject;
    public static bool Coins;
    public static bool Energy;

    private void Awake()
    {
        if (Coins)
        {
            levelObjects.SetActive(false);
            coinsObject.SetActive(true);
        }
        else if (Energy)
        {
            levelObjects.SetActive(false);
            energyObject.SetActive(true);
        }

        Coins = false;
        Energy = false;
    }
}
