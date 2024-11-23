using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour, IPointerClickHandler
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
    [SerializeField] private GameObject waterLily;
    [SerializeField] private Teleport teleport;
    private SpriteRenderer _spriteRenderer;

    [HideInInspector] public Vector2Int gridPosition;
    [HideInInspector] public bool isFeeshOnTile;
    [HideInInspector] public bool isJumperOnTile;
    [HideInInspector] public bool isSonicOnTile;
    public bool AnyCharacterOnTile => isSonicOnTile || isJumperOnTile || isFeeshOnTile;
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
        get => lever.gameObject.activeSelf ? lever.GetSave() : null;
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

    public bool WaterLily
    {
        get => waterLily.activeSelf;
        set => waterLily.SetActive(value);
    }

    public TeleportClass Teleport
    {
        set
        {
            if (value is null)
            {
                teleport.gameObject.SetActive(false);
            }
            else
            {
                teleport.TeleportClass = value;
                teleport.gameObject.SetActive(true);
            }
        }
        get => teleport.gameObject.activeSelf ? teleport.TeleportClass : null;
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
        IsGrass = true;
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

        if (Lever is not null)
        {
            tileObject = OnTileObject.Lever;
        }
        else if (Whirlpool)
        {
            tileObject = OnTileObject.Whirlpool;
        }
        else if (Spike)
        {
            tileObject = OnTileObject.Spike;
        }
        else if (Exit)
        {
            tileObject = OnTileObject.Exit;
        }
        else if (WaterLily)
        {
            tileObject = OnTileObject.WaterLily;
        }
        else if (Teleport is not null)
        {
            tileObject = OnTileObject.Teleport;
        }

        var walls = new Dictionary<Side, WallClass>
        {
            [Side.North] = northWall.GetSave(),
            [Side.South] = southWall.GetSave(),
            [Side.West] = westWall.GetSave(),
            [Side.East] = eastWall.GetSave()
        };
        
        return new TileClass {IsGrass = IsGrass, OnTileObject = tileObject, Walls = walls, LeverClass = Lever, TeleportClass = Teleport};
    }

    public OnTileObject ClearOnTileObject()
    {
        if (Lever is not null)
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
        if (WaterLily)
        {
            WaterLily = false;
            return OnTileObject.WaterLily;
        }
        if (Teleport is not null)
        {
            Teleport = null;
            return OnTileObject.Teleport;
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
        ClearOnTileObject();
        
        IsGrass = tileClass.IsGrass;
        switch (tileClass.OnTileObject)
        {
            case OnTileObject.None:
                break;
            case OnTileObject.Spike:
                Spike = true;
                break;
            case OnTileObject.Whirlpool:
                Whirlpool = true;
                break;
            case OnTileObject.Lever:
                Lever = tileClass.LeverClass;
                break;
            case OnTileObject.Exit:
                Exit = true;
                break;
            case OnTileObject.WaterLily:
                WaterLily = true;
                break;
            case OnTileObject.Teleport:
                Teleport = tileClass.TeleportClass;
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
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.sprite =
                IsGrass ? TileSpriteManager.GetRandomGroundMid : TileSpriteManager.GetRandomWaterMid;
            _spriteRenderer.flipX = Random.value > 0.5;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTileClick.Invoke(this);
    }

    public override bool Equals(object other)
    {
        if (other is Tile tile)
        {
            return Equals(tile);
        }

        return false;
    }
    private bool Equals(Tile other)
    {
        return gridPosition == other.gridPosition;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(gridPosition.x, gridPosition.y);
    }
}
