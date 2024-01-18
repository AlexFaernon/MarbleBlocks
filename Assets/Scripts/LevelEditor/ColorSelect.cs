using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour
{
    public static Dictionary<DoorLeverColor, bool> AllowedColors = new()
    {
        { DoorLeverColor.Red, true },
        { DoorLeverColor.Grey, true },
        { DoorLeverColor.Yellow, true },
        { DoorLeverColor.Green, true },
        { DoorLeverColor.Purple, true },
        { DoorLeverColor.Blue, true },
    };
    private Button _button;
    [SerializeField] private DoorLeverColor color;
    private GameObject _selected;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _selected = transform.GetChild(0).gameObject;
    }
    private void OnClick()
    {
        Brush.Color = color;
    }

    private void Update()
    {
        _selected.SetActive(color == Brush.Color);
        _button.interactable = color switch
        {
            DoorLeverColor.Red or DoorLeverColor.Grey or DoorLeverColor.Yellow => true,
            DoorLeverColor.Purple => LevelObjectsLimits.Purple,
            DoorLeverColor.Green => LevelObjectsLimits.Green,
            DoorLeverColor.Blue => LevelObjectsLimits.Blue,
            _ => throw new ArgumentOutOfRangeException()
        };
        _button.interactable = _button.interactable && AllowedColors[color];
    }
}
