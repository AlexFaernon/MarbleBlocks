using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitchButton : MonoBehaviour
{
    [SerializeField] private string character;
    [SerializeField] private Sprite white;
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
        
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ActivateCharacter);
        switch (character)
        {
            case "Sonic":
                if (CharacterManager.Sonic is null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                CharacterManager.Sonic.switchButton = this;
                break;
            case "Jumper":
                if (CharacterManager.Jumper is null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                CharacterManager.Jumper.switchButton = this;
                break;
            case "Feesh":
                if (CharacterManager.Feesh is null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                CharacterManager.Feesh.switchButton = this;
                break;
        }
    }

    public void ActivateCharacter()
    {
        var sonic = CharacterManager.Sonic;
        var jumper = CharacterManager.Jumper;
        var feesh = CharacterManager.Feesh;
        
        if (sonic.IsMoving || jumper.IsMoving || feesh.IsMoving) return;
        
        if (sonic != null) sonic.IsActive = sonic.CompareTag(character);
        if (jumper != null) jumper.IsActive = jumper.CompareTag(character);
        if (feesh != null) feesh.IsActive = feesh.CompareTag(character);
    }

    private void Update()
    {
        var sonic = CharacterManager.Sonic;
        var jumper = CharacterManager.Jumper;
        var feesh = CharacterManager.Feesh;
        
        var characterActive = character switch
        {
            "Sonic" => sonic.IsActive,
            "Jumper" => jumper.IsActive,
            "Feesh" => feesh.IsActive,
            _ => throw new ArgumentOutOfRangeException("Incorrect character name")
        };
        _image.sprite = characterActive ? white : _normal;
        
        var characterMoving = false;
        if (sonic)
        {
            characterMoving = characterMoving || sonic.IsMoving;
        }
        if (jumper)
        {
            characterMoving = characterMoving || jumper.IsMoving;
        }
        if (feesh)
        {
            characterMoving = characterMoving || feesh.IsMoving;
        }

        _button.interactable = !characterMoving;
    }
}
