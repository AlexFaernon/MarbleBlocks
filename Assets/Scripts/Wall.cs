using System;
using UnityEngine;
using UnityEngine.Events;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isDoor;
    [SerializeField] private bool isOpened;
    [SerializeField] private Color color;
    private SpriteRenderer _spriteRenderer;

    public WallClass WallClass
    {
        set => SetWall(value);
    }

    public static readonly UnityEvent<Color> OnLevelSwitch = new();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        OnLevelSwitch.AddListener(OnLeverSwitch);
    }

    private void OnLeverSwitch(Color leverColor)
    {
        if (color == leverColor)
        {
            isOpened = !isOpened;
        }
    }

    private void Update()
    {
        tag = isDoor ? "Door" : "Wall";

        _spriteRenderer.enabled = true;
        
        _spriteRenderer.color = isDoor ? color : Color.white;
        
        var spriteColor = _spriteRenderer.color;
        color.a = isDoor && isOpened ? 0.5f : 1;
        _spriteRenderer.color = spriteColor;
    }

    public bool AvailableToMove()
    {
        return !gameObject.activeSelf || isDoor && isOpened;
    }

    public WallClass GetSave()
    {
        return new WallClass { IsActive = gameObject.activeSelf, IsDoor = isDoor, Color = color };
    }
    
    private void SetWall(WallClass wallClass)
    {
        gameObject.SetActive(wallClass.IsActive);
        isDoor = wallClass.IsDoor;
        color = wallClass.Color;
    }

    private void OnDestroy()
    {
        OnLevelSwitch.RemoveListener(OnLeverSwitch);
    }
}
