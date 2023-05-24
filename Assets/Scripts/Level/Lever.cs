using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool isSwitchable;
    [SerializeField] private DoorLeverColor color;
    private Sprite _off;
    private Sprite _on;

    public LeverClass LeverClass
    {
        set => SetLever(value);
    }

    private bool _isActive;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _off = TileSpriteManager.GetLeverSprite(color, false);
        _on = TileSpriteManager.GetLeverSprite(color, true);
    }

    private void Update()
    {
        _spriteRenderer.sprite = _isActive ? _on : _off;
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
