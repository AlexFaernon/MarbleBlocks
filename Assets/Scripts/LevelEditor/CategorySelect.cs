using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelect : MonoBehaviour
{
    [SerializeField] private GameObject selected;
    [SerializeField] private Sprite selectedSprite;
    private Image _image;
    private Sprite _unselectedSprite;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _unselectedSprite = _image.sprite;
    }

    private void Update()
    {
        _image.sprite = selected.activeSelf ? selectedSprite : _unselectedSprite;
    }
}
