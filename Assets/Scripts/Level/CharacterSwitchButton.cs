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
    private void OnEnable()
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
        
        _sonic = GameObject.FindWithTag("Sonic")?.GetComponent<Sonic>();
        _jumper = GameObject.FindWithTag("Jumper")?.GetComponent<Jumper>();
        _feesh = GameObject.FindWithTag("Feesh")?.GetComponent<Feesh>();
        
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ActivateCharacter);
        switch (character)
        {
            case "Sonic":
                if (_sonic is null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                _sonic.switchButton = this;
                break;
            case "Jumper":
                if (_jumper is null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                _jumper.switchButton = this;
                break;
            case "Feesh":
                if (_feesh is null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                _feesh.switchButton = this;
                break;
        }
    }

    public void ActivateCharacter()
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
        
        var characterMoving = false;
        if (_sonic)
        {
            characterMoving = characterMoving || _sonic.IsMoving;
        }
        if (_jumper)
        {
            characterMoving = characterMoving || _jumper.IsMoving;
        }
        if (_feesh)
        {
            characterMoving = characterMoving || _feesh.IsMoving;
        }

        _button.interactable = !characterMoving;
    }
}
