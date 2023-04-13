using System;
using UnityEngine;
using UnityEngine.Events;

public class WallClass : MonoBehaviour
{
    [SerializeField] private bool isExisting;
    [SerializeField] private bool isDoor;
    [SerializeField] private bool isOpened;
    [SerializeField] private Color color;
    private SpriteRenderer _spriteRenderer;
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
        if (!isExisting)
        {
            _spriteRenderer.enabled = false;
            return;
        }

        tag = isDoor ? "Door" : "Wall";

        _spriteRenderer.enabled = true;
        
        _spriteRenderer.color = isDoor ? color : Color.white;
        
        var spriteColor = _spriteRenderer.color;
        color.a = isDoor && isOpened ? 0.5f : 1;
        _spriteRenderer.color = spriteColor;
    }

    public bool AvailableToMove()
    {
        return !isExisting || isDoor && isOpened;
    }

    private void OnDestroy()
    {
        OnLevelSwitch.RemoveListener(OnLeverSwitch);
    }
}
