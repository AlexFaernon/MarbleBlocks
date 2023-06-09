using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private Sprite area2Sprite;
    private Sprite _area1Sprite;
    private Image _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<Image>();
        _area1Sprite = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (scroll.verticalNormalizedPosition < 0.5f)
        {
            _spriteRenderer.sprite = _area1Sprite;
        }
        else
        {
            _spriteRenderer.sprite = area2Sprite;
        }
    }
}
