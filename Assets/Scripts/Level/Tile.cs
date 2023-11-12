using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Wall northWall;
    [SerializeField] private Wall southWall;
    [SerializeField] private Wall westWall;
    [SerializeField] private Wall eastWall;
    [SerializeField] private GameObject whirlpool;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject spike;
    [SerializeField] private Lever lever;
    [SerializeField] private GameObject highlight;
    private SpriteRenderer _spriteRenderer;

    [HideInInspector] public Vector2Int gridPosition;
    [HideInInspector] public bool isFeeshOnTile;
    [HideInInspector] public bool isJumperOnTile;
    [HideInInspector] public bool isSonicOnTile;
    public static UnityEvent<Tile> OnTileClick = new ();
    
    public TileClass TileClass
    {
        set => SetLoadedTile(value);
    }

    public bool IsHighlighted
    {
        set => highlight.SetActive(value);
    }

    public LeverClass Lever
    {
        set
        {
            if (value is null)
            {
                lever.gameObject.SetActive(false);
            }
            else
            {
                lever.LeverClass = value;
                lever.gameObject.SetActive(true);
            }
        }
        get => lever.GetSave();
    }

    public bool Spike
    {
        get => spike.activeSelf;
        set => spike.SetActive(value);
    }
    
    public bool Whirlpool
    {
        get => whirlpool.activeSelf;
        set => whirlpool.SetActive(value);
    }

    public bool Exit
    {
        get => exit.activeSelf;
        set => exit.SetActive(value);
    }
    
    public bool IsEdge { get; set; }
    
    private bool _isGrass;
    public bool IsGrass
    {
        get => _isGrass;
        set
        {
            _isGrass = value;
            SetSprite();
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsEdge)
        {
            _spriteRenderer.color = Color.clear;
        }
    }

    public Wall GetWall(Side side)
    {
        return side switch
        {
            Side.North => northWall,
            Side.South => southWall,
            Side.West => westWall,
            Side.East => eastWall,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
    }

    public bool AvailableToMoveThroughSide(Side side)
    {
        return side switch
        {
            Side.North => northWall.AvailableToMove(),
            Side.South => southWall.AvailableToMove(),
            Side.West => westWall.AvailableToMove(),
            Side.East => eastWall.AvailableToMove(),
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
    }
    
    public TileClass GetSave()
    {
        var tileObject = OnTileObject.None;

        if (lever.gameObject.activeSelf)
        {
            tileObject = OnTileObject.Lever;
        }
        else if (whirlpool.activeSelf)
        {
            tileObject = OnTileObject.Whirlpool;
        }
        else if (spike.activeSelf)
        {
            tileObject = OnTileObject.Spike;
        }
        else if (exit.activeSelf)
        {
            tileObject = OnTileObject.Exit;
        }

        var walls = new Dictionary<Side, WallClass>
        {
            [Side.North] = northWall.GetSave(),
            [Side.South] = southWall.GetSave(),
            [Side.West] = westWall.GetSave(),
            [Side.East] = eastWall.GetSave()
        };

        var leverClass = tileObject == OnTileObject.Lever ? lever.GetSave() : null;
        
        return new TileClass {IsGrass = IsGrass, OnTileObject = tileObject, Walls = walls, LeverClass = leverClass};
    }

    public OnTileObject ClearOnTileObject()
    {
        if (lever.gameObject.activeSelf)
        {
            Lever = null;
            return OnTileObject.Lever;
        }
        if (Exit)
        {
            Exit = false;
            return OnTileObject.Exit;
        }
        if (Spike)
        {
            Spike = false;
            return OnTileObject.Spike;
        }
        if (Whirlpool)
        {
            Whirlpool = false;
            return OnTileObject.Whirlpool;
        }

        return OnTileObject.None;
    }

    public void ClearTile()
    {
        IsGrass = true;
        northWall.WallClass = new WallClass();
        southWall.WallClass = new WallClass();
        eastWall.WallClass = new WallClass();
        westWall.WallClass = new WallClass();
        ClearOnTileObject();
    }

    private void SetLoadedTile(TileClass tileClass)
    {
        IsGrass = tileClass.IsGrass;
        switch (tileClass.OnTileObject)
        {
            case OnTileObject.None:
                break;
            case OnTileObject.Spike:
                spike.SetActive(true);
                break;
            case OnTileObject.Whirlpool:
                whirlpool.SetActive(true);
                break;
            case OnTileObject.Lever:
                lever.LeverClass = tileClass.LeverClass;
                lever.gameObject.SetActive(true);
                break;
            case OnTileObject.Exit:
                exit.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        SetSprite();

        northWall.WallClass = tileClass.Walls[Side.North];
        southWall.WallClass = tileClass.Walls[Side.South];
        westWall.WallClass = tileClass.Walls[Side.West];
        eastWall.WallClass = tileClass.Walls[Side.East];
    }

    private void SetSprite()
    {
        if (gridPosition.y == 1)
        {
            _spriteRenderer.sprite =
                IsGrass ? TileSpriteManager.GetRandomGroundBot : TileSpriteManager.GetRandomWaterBot;
        }
        else
        {
            _spriteRenderer.sprite =
                IsGrass ? TileSpriteManager.GetRandomGroundMid : TileSpriteManager.GetRandomWaterMid;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Feesh"))
        {
            isFeeshOnTile = true;
        }

        if (col.CompareTag("Jumper"))
        {
            isJumperOnTile = true;
        }
        
        if (col.CompareTag("Sonic"))
        {
            isSonicOnTile = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Feesh"))
        {
            isFeeshOnTile = false;
        }

        if (col.CompareTag("Jumper"))
        {
            isJumperOnTile = false;
        }

        if (col.CompareTag("Sonic"))
        {
            isSonicOnTile = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTileClick.Invoke(this);
    }
}
