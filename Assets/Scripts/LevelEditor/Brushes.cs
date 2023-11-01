using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Brushes : MonoBehaviour
{
    [SerializeField] private GameObject feesh;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject sonic;
    private static bool _isGrass;
    private static OnTileObject _onTileObject;
    public static Side Side;
    public static DoorLeverColor Color;
    private static bool _gateOpened;
    private static GameObject _selectedCharacter;
    private static Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public void PlaceGrass()
    {
        _isGrass = true;
        Drawer.CurrentBrush = ChangeGround;
    }

    public void PlaceWater()
    {
        _isGrass = false;
        Drawer.CurrentBrush = ChangeGround;
    }

    public void PlaceLever()
    {
        _onTileObject = OnTileObject.Lever;
        Drawer.CurrentBrush = PlaceObject;
    }

    public void PlaceExit()
    {
        _onTileObject = OnTileObject.Exit;
        Drawer.CurrentBrush = PlaceObject;
    }

    public void PlaceSpike()
    {
        _onTileObject = OnTileObject.Spike;
        Drawer.CurrentBrush = PlaceObject;
    }

    public void PlaceWhirlpool()
    {
        _onTileObject = OnTileObject.Whirlpool;
        Drawer.CurrentBrush = PlaceObject;
    }

    public void PlaceWall()
    {
        Drawer.CurrentBrush = PlaceWall;
    }

    public void PlaceClosedGate()
    {
        _gateOpened = false;
        Drawer.CurrentBrush = PlaceGates;
    }
    
    public void PlaceOpenedGate()
    {
        _gateOpened = true;
        Drawer.CurrentBrush = PlaceGates;
    }

    public void RemoveObject()
    {
         Drawer.CurrentBrush = EraseObject;
    }

    public void PlaceFeesh()
    {
        _selectedCharacter = feesh;
        Drawer.CurrentBrush = PlaceCharacter;
    }
    
    public void PlaceJumper()
    {
        _selectedCharacter = jumper;
        Drawer.CurrentBrush = PlaceCharacter;
    }
    
    public void PlaceSonic()
    {
        _selectedCharacter = sonic;
        Drawer.CurrentBrush = PlaceCharacter;
    }

    private static Action ChangeGround(Tile tile)
    {
        var isGrass = !_isGrass;
        Action redo = () => tile.IsGrass = isGrass;
        tile.IsGrass = _isGrass;
        return redo;
    }

    private static Action PlaceObject(Tile tile)
    {
        Action redo;
        var redoErase = EraseObject(tile);
        switch (_onTileObject)
        {
            case OnTileObject.None:
                throw new ArgumentException();
            case OnTileObject.Spike:
                tile.Spike = true;
                redo = () => tile.Spike = false;
                break;
            case OnTileObject.Whirlpool:
                tile.Whirlpool = true;
                redo = () => tile.Whirlpool = false;
                break;
            case OnTileObject.Lever:
                tile.Lever = new LeverClass
                {
                    IsSwitchable = false,
                    Color = Color
                };
                redo = () => tile.Lever = null;
                break;
            case OnTileObject.Exit:
                tile.Exit = true;
                redo = () => tile.Exit = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return redo + redoErase;
    }

    private static Action PlaceWall(Tile tile)
    {
        var wall = tile.GetWall(Side);
        var oldWall = wall.GetSave();
        Action redo = () => wall.WallClass = oldWall;
        wall.WallClass = new WallClass
        {
            IsActive = true,
            IsDoor = false,
            IsOpened = false,
            Color = Color
        };
        return redo;
    }

    private static Action PlaceGates(Tile tile)
    {
        var wall = tile.GetWall(Side);
        var oldWall = wall.GetSave();
        wall.WallClass = new WallClass
        {
            IsActive = true,
            IsDoor = true,
            IsOpened = _gateOpened,
            Color = Color
        };
        Action redo = () => wall.WallClass = oldWall;
        return redo;
    }

    private Action PlaceCharacter(Tile tile)
    {
        Action redo;
        var pos = _grid.GetCellCenterWorld((Vector3Int)tile.gridPosition);
        var character = Instantiate(_selectedCharacter, pos, quaternion.identity);
        redo = () => Destroy(character);
        if (_selectedCharacter == sonic)
        {
            character.GetComponent<Sonic>().enabled = false;
        }
        else if (_selectedCharacter == feesh)
        {
            character.GetComponent<Feesh>().enabled = false;
        }
        else if (_selectedCharacter == jumper)
        {
            character.GetComponent<Jumper>().enabled = false;
        }
        return redo;
    }

    private static Action EraseObject(Tile tile)
    {
        Action redo;
        var lever = tile.GetSave().LeverClass;
        if (lever != null)
        {
            tile.Lever = null;
            redo = () => tile.Lever = lever;
            return redo;
        }
        if (tile.Exit)
        {
            tile.Exit = false;
            redo = () => tile.Exit = true;
            return redo;
        }
        if (tile.Spike)
        {
            tile.Spike = false;
            redo = () => tile.Spike = true;
            return redo;
        }
        if (tile.Whirlpool)
        {
            tile.Whirlpool = false;
            redo = () => tile.Whirlpool = true;
            return redo;
        }
        return null;
    }
}
