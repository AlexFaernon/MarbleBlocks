using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitchButton : MonoBehaviour
{
    [SerializeField] private string character;
    private Sonic sonic;
    private Jumper jumper;
    private Feesh feesh;
    private Button button;
    private void Awake()
    {
        var sonicObj = GameObject.FindWithTag("Sonic");
        if (sonicObj)
        {
            sonic =  sonicObj.GetComponent<Sonic>();
        }
        var jumperObj = GameObject.FindWithTag("Jumper");
        if (jumperObj)
        {
            jumper = jumperObj.GetComponent<Jumper>();
        }
        
        var feeshObj = GameObject.FindWithTag("Feesh");
        if (feeshObj)
        {
            feesh = feeshObj.GetComponent<Feesh>();
        }
        
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateCharacter);
        switch (character)
        {
            case "Sonic":
                if (sonic is null)
                {
                    gameObject.SetActive(false);
                }
                return;
            case "Jumper":
                if (jumper is null)
                {
                    gameObject.SetActive(false);
                }
                return;
            case "Feesh":
                if (feesh is null)
                {
                    gameObject.SetActive(false);
                }
                return;
        }
    }

    private void ActivateCharacter()
    {
        if (sonic != null) sonic.IsActive   = sonic.CompareTag(character);
        if (jumper != null) jumper.IsActive = jumper.CompareTag(character);
        if (feesh != null) feesh.IsActive   = feesh.CompareTag(character);
    }

    private void Update()
    {
        button.interactable = character switch
        {
            "Sonic" => !sonic.IsActive,
            "Jumper" => !jumper.IsActive,
            "Feesh" => !feesh.IsActive,
            _ => throw new ArgumentOutOfRangeException("Incorrect character name")
        };
        if (sonic != null) button.interactable = !sonic.IsMoving;
    }
}
