using System;
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
        void GroundBrush(Tile tile) => ChangeGround(tile, _isGrass);
        Drawer.CurrentBrush = GroundBrush;
    }
    
    public void PlaceWater()
    {
        _isGrass = false;
        void GroundBrush(Tile tile) => ChangeGround(tile, _isGrass);
        Drawer.CurrentBrush = GroundBrush;
    }
    
    public void PlaceLever()
    {
        _onTileObject = OnTileObject.Lever;
        Drawer.CurrentBrush = tile => PlaceObject(tile, _onTileObject, null);
    }
    
    public void PlaceExit()
    {
        _onTileObject = OnTileObject.Exit;
        Drawer.CurrentBrush = tile => PlaceObject(tile, _onTileObject, null);
    }
    
    public void PlaceSpike()
    {
        _onTileObject = OnTileObject.Spike;
        Drawer.CurrentBrush = tile => PlaceObject(tile, _onTileObject, null);
    }
    
    public void PlaceWhirlpool()
    {
        _onTileObject = OnTileObject.Whirlpool;
        Drawer.CurrentBrush = tile => PlaceObject(tile, _onTileObject, null);
    }

    public void PlaceWall()
    {
        void WallBrush(Tile tile) => PlaceWall(tile, Side);
        Drawer.CurrentBrush = WallBrush;
    }

    public void PlaceClosedGate()
    {
        _gateOpened = false;
        Drawer.CurrentBrush = tile => PlaceGates(tile, Side, null);
    }
    
    public void PlaceOpenedGate()
    {
        _gateOpened = true;
        Drawer.CurrentBrush = tile => PlaceGates(tile, Side, null);
    }
    
    public void RemoveObject()
    {
         Drawer.CurrentBrush = EraseObject;
    }
    
    public void PlaceFeesh()
    {
        _selectedCharacter = feesh;
        Drawer.CurrentBrush = tile => PlaceCharacter(tile, _selectedCharacter);
    }
    
    public void PlaceJumper()
    {
        _selectedCharacter = jumper;
        Drawer.CurrentBrush = tile => PlaceCharacter(tile, _selectedCharacter);
    }
    
    public void PlaceSonic()
    {
        _selectedCharacter = sonic;
        Drawer.CurrentBrush = tile => PlaceCharacter(tile, _selectedCharacter);
    }

    private static void ChangeGround(Tile tile, bool isGrass)
    {
        tile.IsGrass = isGrass;
        void Undo() => RevertGround(tile, isGrass);
        Drawer.Undo.Push(Undo);
    }

    private static void RevertGround(Tile tile, bool isGrass)
    {
        tile.IsGrass = !isGrass;
        void Redo() => ChangeGround(tile, isGrass);
        Drawer.Redo.Push(Redo);
    }

    private static void PlaceObject(Tile tile, OnTileObject onTileObject, LeverClass lever)
    {
        var oldLever = tile.Lever;
        var oldOnTileObject = tile.ClearOnTileObject();
        switch (onTileObject)
        {
            case OnTileObject.None:
                throw new ArgumentOutOfRangeException();
            case OnTileObject.Spike:
                tile.Spike = true;
                break;
            case OnTileObject.Whirlpool:
                tile.Whirlpool = true;
                break;
            case OnTileObject.Lever:
                lever ??= new LeverClass
                {
                    IsSwitchable = false,
                    Color = Color
                };
                tile.Lever = lever;
                break;
            case OnTileObject.Exit:
                tile.Exit = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Drawer.Undo.Push(() => RevertObject(tile, oldOnTileObject, oldLever));
    }

    private static void RevertObject(Tile tile, OnTileObject oldOnTileObject, LeverClass oldLever)
    {
        var lever = tile.Lever;
        var onTileObject = tile.ClearOnTileObject();
        switch (oldOnTileObject)
        {
            case OnTileObject.None:
                break;
            case OnTileObject.Spike:
                tile.Spike = true;
                break;
            case OnTileObject.Whirlpool:
                tile.Whirlpool = true;
                break;
            case OnTileObject.Lever:
                tile.Lever = oldLever;
                break;
            case OnTileObject.Exit:
                tile.Exit = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Drawer.Redo.Push(() => PlaceObject(tile, onTileObject, lever));
    }
    
    private static void PlaceWall(Tile tile, Side side)
    {
        var wall = tile.GetWall(side);
        var oldWall = wall.GetSave();
        wall.WallClass = new WallClass
        {
            IsActive = true,
            IsDoor = false,
            IsOpened = false,
            Color = Color
        };
        
        void Undo() => RevertWall(tile, side, oldWall);
        Drawer.Undo.Push(Undo);
    }

    private static void RevertWall(Tile tile, Side side, WallClass oldWall)
    {
        var wall = tile.GetWall(side);
        wall.WallClass = oldWall;
        void Redo() => PlaceWall(tile, side);
        Drawer.Redo.Push(Redo);
    }

    private static void PlaceGates(Tile tile, Side side, WallClass wallClass)
    {
        var wall = tile.GetWall(side);
        var oldWall = wall.GetSave();
        wallClass ??= new WallClass
        {
            IsActive = true,
            IsDoor = true,
            IsOpened = _gateOpened,
            Color = Color
        };
        wall.WallClass = wallClass;
        
        Drawer.Undo.Push(() => RevertGates(tile, side, oldWall));
    }

    private static void RevertGates(Tile tile, Side side, WallClass oldWall)
    {
        var wall = tile.GetWall(side);
        var newWall = wall.GetSave();
        wall.WallClass = oldWall;
        
        Drawer.Redo.Push(() => PlaceGates(tile, side, newWall));
    }

    private void PlaceCharacter(Tile tile, GameObject selectedCharacter)
    {
        var pos = _grid.GetCellCenterWorld((Vector3Int)tile.gridPosition);
        var character = Instantiate(selectedCharacter, pos, quaternion.identity);
        if (selectedCharacter == sonic)
        {
            character.GetComponent<Sonic>().enabled = false;
        }
        else if (selectedCharacter == feesh)
        {
            character.GetComponent<Feesh>().enabled = false;
        }
        else if (selectedCharacter == jumper)
        {
            character.GetComponent<Jumper>().enabled = false;
        }
        
        Drawer.Undo.Push(() => RemoveCharacter(tile, selectedCharacter, character));
    }

    private void RemoveCharacter(Tile tile, GameObject selectedCharacter, GameObject characterToRemove)
    {
        Destroy(characterToRemove);
        Drawer.Redo.Push(() => PlaceCharacter(tile, selectedCharacter));
    }

    private void EraseObject(Tile tile)
    {
        var oldTile = tile.GetSave();
        tile.ClearTile();
        GameObject character = null;
        if (tile.isSonicOnTile)
        {
            Destroy(GameObject.FindWithTag("Sonic"));
            character = sonic;
        }
        if (tile.isJumperOnTile)
        {
            Destroy(GameObject.FindWithTag("Jumper"));
            character = jumper;
        }
        if (tile.isFeeshOnTile)
        {
            Destroy(GameObject.FindWithTag("Feesh"));
            character = feesh;
        }
        
        Drawer.Undo.Push(() => RevertErase(tile, oldTile, character));
    }

    private void RevertErase(Tile tile, TileClass oldTile, GameObject selectedCharacter)
    {
        tile.TileClass = oldTile;
        if (selectedCharacter is not null)
        {
            var pos = _grid.GetCellCenterWorld((Vector3Int)tile.gridPosition);
            var character = Instantiate(selectedCharacter, pos, quaternion.identity);
            if (selectedCharacter == sonic)
            {
                character.GetComponent<Sonic>().enabled = false;
            }
            else if (selectedCharacter == feesh)
            {
                character.GetComponent<Feesh>().enabled = false;
            }
            else if (selectedCharacter == jumper)
            {
                character.GetComponent<Jumper>().enabled = false;
            }
        }
        Drawer.Redo.Push(() => EraseObject(tile));
    }
}
