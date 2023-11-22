using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] private GameObject register;
    [SerializeField] private GameObject login;

    public static UISwitcher Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this);
        }

        if (AuthManager.auth == null) return;
        register.SetActive(false);
        login.SetActive(false);
    }

    public void LoginOn()
    {
        register.SetActive(false);
        login.SetActive(true);
    }

    public void CloseLogin()
    {
        login.SetActive(false);
    }
}
