using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectMenu : MonoBehaviour
{
    [SerializeField] private Image colorIcon;
    [SerializeField] private GameObject colorButtons;
    [SerializeField] private Sprite red;
    [SerializeField] private Sprite grey;
    [SerializeField] private Sprite purple;
    [SerializeField] private Sprite blue;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite yellow;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Update()
    {
        colorIcon.sprite = Brushes.Color switch
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

    private void OnClick()
    {
        colorButtons.SetActive(!colorButtons.activeSelf);
    }
}