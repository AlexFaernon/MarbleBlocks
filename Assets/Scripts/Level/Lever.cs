using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private bool isSwitchable = true;
    [SerializeField] private DoorLeverColor color;
    public static int Count;
    private Sprite _off;
    private Sprite _on;

    public LeverClass LeverClass
    {
        set => SetLever(value);
    }

    public bool IsActive { private set; get; }
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.sprite = IsActive ? _on : _off;
    }

    public LeverClass GetSave()
    {
        return new LeverClass { Color = color, IsSwitchable = isSwitchable };
    }

    public void Switch()
    {
        if (!isSwitchable && IsActive) return;
        
        IsActive = !IsActive;
        Wall.OnLevelSwitch.Invoke(color);
    }
    
    private void SetLever(LeverClass leverClass)
    {
        IsActive = false;
        isSwitchable = leverClass.IsSwitchable;
        color = leverClass.Color;
        _off = Resources.Load<Sprite>($"Levers/Off/{color}");
        _on = Resources.Load<Sprite>($"Levers/On/{color}");
    }
    
    private void OnEnable()
    {
        Count++;
    }

    private void OnDisable()
    {
        Count--;
    }
}
