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
    private static Side _side;
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

    public void PlaceNorthWall()
    {
        _side = Side.North;
        Drawer.CurrentBrush = PlaceWall;
    }
    
    public void PlaceSouthWall()
    {
        _side = Side.South;
        Drawer.CurrentBrush = PlaceWall;
    }
    
    public void PlaceWestWall()
    {
        _side = Side.West;
        Drawer.CurrentBrush = PlaceWall;
    }
    
    public void PlaceEastWall()
    {
        _side = Side.East;
        Drawer.CurrentBrush = PlaceWall;
    }

    // public void RemoveObject()
    // {
    //     BrushManager.CurrentBrush = EraseObject;
    // }

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
        EraseObject(tile);
        switch (_onTileObject)
        {
            case OnTileObject.None:
                throw new ArgumentException();
            case OnTileObject.Spike:
                tile.Spike = true;
                redo = () => tile.Spike = false;
                return redo;
            case OnTileObject.Whirlpool:
                tile.Whirlpool = true;
                redo = () => tile.Whirlpool = false;
                return redo;
            case OnTileObject.Lever:
                return null;
            case OnTileObject.Exit:
                tile.Exit = true;
                redo = () => tile.Exit = false;
                return redo;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static Action PlaceWall(Tile tile)
    {
        var savedSide = _side;
        Action redo = () => tile.GetWall(savedSide).gameObject.SetActive(false);
        tile.GetWall(_side).gameObject.SetActive(true);
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

    private static void EraseObject(Tile tile)
    {
        tile.Lever = null;
        tile.Exit = false;
        tile.Spike = false;
        tile.Whirlpool = false;
    }
}
