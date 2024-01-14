using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour
{
    private Button _button;
    [SerializeField] private DoorLeverColor color;
    private GameObject _selected;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _selected = transform.GetChild(0).gameObject;
        gameObject.SetActive(color switch
        {
            DoorLeverColor.Red or DoorLeverColor.Grey or DoorLeverColor.Blue => true,
            DoorLeverColor.Purple => LevelObjectsLimits.Purple,
            DoorLeverColor.Green => LevelObjectsLimits.Green,
            DoorLeverColor.Yellow => LevelObjectsLimits.Yellow,
            _ => throw new ArgumentOutOfRangeException()
        });
    }
    private void OnClick()
    {
        Brush.Color = color;
        Brush.Color = color;
    }

    private void Update()
    {
        _selected.SetActive(color == Brush.Color);
    }
}
