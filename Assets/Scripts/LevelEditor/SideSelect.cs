using System;
using UnityEngine;
using UnityEngine.UI;

public class SideSelect : MonoBehaviour
{
    [SerializeField] private GameObject unselected;
    [SerializeField] private GameObject selected;
    private Sprite _unselected;
    private Button _button;
    [SerializeField] private Side side;
    private Image _image;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Brush.Side = side;
    }

    private void Update()
    {
        unselected.SetActive(Brush.Side != side);
        selected.SetActive(Brush.Side == side);
    }
}
