using System;
using UnityEngine;
using UnityEngine.UI;

public class SideSelect : MonoBehaviour
{
    [SerializeField] private string stringColor;
    [SerializeField] private Sprite selected;
    private Sprite _unselected;
    private Button _button;
    private Side _side;
    private Image _image;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        _side = Enum.Parse<Side>(stringColor);
        _image = GetComponent<Image>();
        _unselected = _image.sprite;
    }

    private void OnClick()
    {
        Brushes.Side = _side;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    private void Update()
    {
        _image.sprite = Brushes.Side == _side ? selected : _unselected;
    }
}
