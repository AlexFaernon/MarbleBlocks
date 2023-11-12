using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowColorInObjects : MonoBehaviour
{
    [SerializeField] private Sprite red;
    [SerializeField] private Sprite purple;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite grey;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite blue;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        _image.sprite = Brush.Color switch
        {
            DoorLeverColor.Red => red,
            DoorLeverColor.Grey => grey,
            DoorLeverColor.Blue => blue,
            DoorLeverColor.Purple => purple,
            DoorLeverColor.Green => green,
            DoorLeverColor.Yellow => yellow,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
