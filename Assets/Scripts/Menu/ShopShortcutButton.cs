using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopShortcutButton : MonoBehaviour
{
    [SerializeField] private bool energy;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            {
                ShopShortcut.Energy = energy;
                ShopShortcut.Coins = !energy;
                SceneManager.LoadScene("Shop");
            });
    }
}
