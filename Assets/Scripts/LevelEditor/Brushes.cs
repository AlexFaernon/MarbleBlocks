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
        BrushManager.CurrentBrush = ChangeGround;
    }

    public void PlaceWater()
    {
        _isGrass = false;
        BrushManager.CurrentBrush = ChangeGround;
    }

    public void PlaceExit()
    {
        _onTileObject = OnTileObject.Exit;
        BrushManager.CurrentBrush = PlaceObject;
    }

    public void PlaceSpike()
    {
        _onTileObject = OnTileObject.Spike;
        BrushManager.CurrentBrush = PlaceObject;
    }

    public void PlaceWhirlpool()
    {
        _onTileObject = OnTileObject.Whirlpool;
        BrushManager.CurrentBrush = PlaceObject;
    }

    public void PlaceNorthWall()
    {
        _side = Side.North;
        BrushManager.CurrentBrush = PlaceWall;
    }
    
    public void PlaceSouthWall()
    {
        _side = Side.South;
        BrushManager.CurrentBrush = PlaceWall;
    }
    
    public void PlaceWestWall()
    {
        _side = Side.West;
        BrushManager.CurrentBrush = PlaceWall;
    }
    
    public void PlaceEastWall()
    {
        _side = Side.East;
        BrushManager.CurrentBrush = PlaceWall;
    }

    public void RemoveObject()
    {
        BrushManager.CurrentBrush = EraseObject;
    }

    public void PlaceFeesh()
    {
        _selectedCharacter = feesh;
        BrushManager.CurrentBrush = PlaceCharacter;
    }
    
    public void PlaceJumper()
    {
        _selectedCharacter = jumper;
        BrushManager.CurrentBrush = PlaceCharacter;
    }
    
    public void PlaceSonic()
    {
        _selectedCharacter = sonic;
        BrushManager.CurrentBrush = PlaceCharacter;
    }

    private static void ChangeGround(Tile tile)
    {
        tile.IsGrass = _isGrass;
    }

    private static void PlaceObject(Tile tile)
    {
        EraseObject(tile);
        switch (_onTileObject)
        {
            case OnTileObject.None:
                throw new ArgumentException();
            case OnTileObject.Spike:
                tile.Spike = true;
                break;
            case OnTileObject.Whirlpool:
                tile.Whirlpool = true;
                break;
            case OnTileObject.Lever:
                break;
            case OnTileObject.Exit:
                tile.Exit = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void PlaceWall(Tile tile)
    {
        tile.GetWall(_side).gameObject.SetActive(true);
    }

    private void PlaceCharacter(Tile tile)
    {
        var pos = _grid.GetCellCenterWorld((Vector3Int)tile.gridPosition);
        var character = Instantiate(_selectedCharacter, pos, quaternion.identity);
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
    }

    private static void EraseObject(Tile tile)
    {
        tile.Lever = null;
        tile.Exit = false;
        tile.Spike = false;
        tile.Whirlpool = false;
    }
}
