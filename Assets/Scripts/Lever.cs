using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool isSwitchable;
    [SerializeField] private Color color;
    private bool _isActive;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.color = color;
        
        var spriteColor = _spriteRenderer.color;
        color.a = _isActive ? 0.5f : 1;
        _spriteRenderer.color = spriteColor;
    }

    public void Switch()
    {
        if (!isSwitchable && _isActive) return;
        
        _isActive = !_isActive;
        WallClass.OnLevelSwitch.Invoke(color);
    }
}
