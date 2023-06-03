using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button confirm;

    public static string PlayerName = "";

    private void Awake()
    {
        Debug.Log(PlayerName);
        if (PlayerName != "")
        {
            gameObject.SetActive(false);
        }
        confirm.onClick.AddListener(Confirm);
    }

    private void Update()
    {
        confirm.interactable = inputField.text != "";
    }

    private void Confirm()
    {
        PlayerName = inputField.text;
        gameObject.SetActive(false);
    }
}
