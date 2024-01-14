using System;
using UnityEngine;
using UnityEngine.Events;

public class Wall : MonoBehaviour
{
    [SerializeField] private bool isDoor;
    [SerializeField] private bool isOpened;
    [SerializeField] private DoorLeverColor color;
    public static int DoorCount { get; private set; }
    private SpriteRenderer _spriteRenderer;
    private Sprite _closed;
    private Sprite _opened;
    private Sprite _wall;

    public WallClass WallClass
    {
        set => SetWall(value);
    }

    public static readonly UnityEvent<DoorLeverColor> OnLevelSwitch = new();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _wall = _spriteRenderer.sprite;
    }

    private void OnLeverSwitch(DoorLeverColor leverColor)
    {
        if (color == leverColor)
        {
            isOpened = !isOpened;
        }
    }

    private void Update()
    {
        if (isDoor)
        {
            _spriteRenderer.sprite = isOpened ? _opened : _closed;
        }
    }

    public bool AvailableToMove()
    {
        return !gameObject.activeSelf || isDoor && isOpened;
    }

    public WallClass GetSave()
    {
        return new WallClass { IsActive = gameObject.activeSelf, IsDoor = isDoor, IsOpened = isOpened, Color = color };
    }
    
    private void SetWall(WallClass wallClass)
    {
        if (wallClass.IsActive)
        {
            if (wallClass.IsDoor)
            {
                DoorCount++;
            }
        }
        else
        {
            if (isDoor)
            {
                DoorCount--;
            }
        }
        OnLevelSwitch.RemoveListener(OnLeverSwitch);
        gameObject.SetActive(wallClass.IsActive);
        isDoor = wallClass.IsDoor;
        color = wallClass.Color;
        
        if (isDoor)
        {
            isOpened = wallClass.IsOpened;
            var position = transform.localPosition.x == 0 ? "Horizontal" : "Vertical";
            _opened = Resources.Load<Sprite>($"Doors/{position}/Opened/{color}");
            _closed = Resources.Load<Sprite>($"Doors/{position}/Closed/{color}");
            OnLevelSwitch.AddListener(OnLeverSwitch);
        }
        else
        {
            if (_spriteRenderer)
                _spriteRenderer.sprite = _wall;
        }
        
        tag = isDoor ? "Door" : "Wall";
    }

    private void OnDisable()
    {
        OnLevelSwitch.RemoveListener(OnLeverSwitch);
    }
}
