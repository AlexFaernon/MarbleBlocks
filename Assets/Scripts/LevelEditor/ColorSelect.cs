using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelect : MonoBehaviour
{
    [SerializeField] private string stringColor;
    private Button _button;
    private DoorLeverColor _color;
    private GameObject _selected;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _color = Enum.Parse<DoorLeverColor>(stringColor);
        _selected = transform.GetChild(0).gameObject;
    }
    private void OnClick()
    {
        Brushes.Color = _color;
    }

    private void Update()
    {
        _selected.SetActive(_color == Brushes.Color);
    }
}
