using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool isSwitchable;
    [SerializeField] private Color color;

    public LeverClass LeverClass
    {
        set => SetLever(value);
    }

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

    public LeverClass GetSave()
    {
        return new LeverClass { Color = color, IsSwitchable = isSwitchable };
    }

    public void Switch()
    {
        if (!isSwitchable && _isActive) return;
        
        _isActive = !_isActive;
        Wall.OnLevelSwitch.Invoke(color);
    }
    
    private void SetLever(LeverClass leverClass)
    {
        isSwitchable = leverClass.IsSwitchable;
        color = leverClass.Color;
    }
}
