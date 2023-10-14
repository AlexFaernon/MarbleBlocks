using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brushes : MonoBehaviour
{
    private static bool _isGrass;
    private static OnTileObject _onTileObject;
    private static Side _side;
    
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

    private static void EraseObject(Tile tile)
    {
        tile.Lever = null;
        tile.Exit = false;
        tile.Spike = false;
        tile.Whirlpool = false;
    }
}
