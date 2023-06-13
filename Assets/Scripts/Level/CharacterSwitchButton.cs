using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitchButton : MonoBehaviour
{
    [SerializeField] private string character;
    [SerializeField] private Sprite white;
    private Sonic _sonic;
    private Jumper _jumper;
    private Feesh _feesh;
    private Button _button;
    private Image _image;
    private Sprite _normal;
    private void Awake()
    {
        switch (LevelSaveManager.LevelNumber)
        {
            case 2 when character != "Sonic":
                gameObject.SetActive(false);
                return;
            case 3 when character != "Jumper":
                gameObject.SetActive(false);
                return;
        }
        _image = GetComponent<Image>();
        _normal = _image.sprite;

        var sonicObj = GameObject.FindWithTag("Sonic");
        if (sonicObj)
        {
            _sonic =  sonicObj.GetComponent<Sonic>();
        }
        var jumperObj = GameObject.FindWithTag("Jumper");
        if (jumperObj)
        {
            _jumper = jumperObj.GetComponent<Jumper>();
        }
        
        var feeshObj = GameObject.FindWithTag("Feesh");
        if (feeshObj)
        {
            _feesh = feeshObj.GetComponent<Feesh>();
        }
        
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ActivateCharacter);
        switch (character)
        {
            case "Sonic":
                if (_sonic is null)
                {
                    gameObject.SetActive(false);
                }
                return;
            case "Jumper":
                if (_jumper is null)
                {
                    gameObject.SetActive(false);
                }
                return;
            case "Feesh":
                if (_feesh is null)
                {
                    gameObject.SetActive(false);
                }
                return;
        }
    }

    private void ActivateCharacter()
    {
        if (_sonic != null) _sonic.IsActive   = _sonic.CompareTag(character);
        if (_jumper != null) _jumper.IsActive = _jumper.CompareTag(character);
        if (_feesh != null) _feesh.IsActive   = _feesh.CompareTag(character);
    }

    private void Update()
    {
        var characterActive = character switch
        {
            "Sonic" => _sonic.IsActive,
            "Jumper" => _jumper.IsActive,
            "Feesh" => _feesh.IsActive,
            _ => throw new ArgumentOutOfRangeException("Incorrect character name")
        };
        transform.localScale = characterActive ? new Vector3(1.2f, 1.2f, 1.2f) : Vector3.one;
        _image.sprite = characterActive ? white : _normal;
        if (_sonic != null) _button.interactable = !_sonic.IsMoving && !_feesh.IsMoving && !_jumper.IsMoving;
    }
}
