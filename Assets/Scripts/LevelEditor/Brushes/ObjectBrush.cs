using System;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBrush : Brush
{
    [SerializeField] private OnTileObject selectedOnTileObject;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        Image = GetComponent<Image>();
        UnselectedSprite = Image.sprite;
    }

    private void Update()
    {
        Image.sprite = Drawer.CurrentBrush == this ? selectedSprite : UnselectedSprite;
        
        Button.interactable = selectedOnTileObject switch
        {
            OnTileObject.None => throw new ArgumentOutOfRangeException(),
            OnTileObject.Spike => Spike.Count < LevelObjectsLimits.Spike,
            OnTileObject.Whirlpool => Whirlpool.Count < LevelObjectsLimits.Whirlpool,
            OnTileObject.Lever => Lever.Count < LevelObjectsLimits.Lever,
            OnTileObject.Exit => Exit.Count < LevelObjectsLimits.Exit,
            OnTileObject.WaterLily => WaterLily.Count < LevelObjectsLimits.WaterLily,
            OnTileObject.Teleport => Teleport.Count < LevelObjectsLimits.Teleport,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override bool Draw(Tile tile)
	{
        if (!Button.interactable) return false;
        
        if (Check(tile, selectedOnTileObject))
        {
            PlaceObject(tile, selectedOnTileObject, null, null);
            return true;
        }

        return false;
    }
	
	private void PlaceObject(Tile tile, OnTileObject onTileObject, LeverClass lever, TeleportClass teleport)
    {
        var oldLever = tile.Lever;
        var oldTeleport = tile.Teleport;
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
                    IsSwitchable = true,
                    Color = Color
                };
                tile.Lever = lever;
                break;
            case OnTileObject.Exit:
                tile.Exit = true;
                break;
            case OnTileObject.WaterLily:
                tile.WaterLily = true;
                break;
            case OnTileObject.Teleport:
                teleport ??= new TeleportClass
                {
                    Color = Color
                };
                tile.Teleport = teleport;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
            
        Drawer.Undo.Push(() => RevertObject(tile, oldOnTileObject, oldLever, oldTeleport));
    }
    
    private void RevertObject(Tile tile, OnTileObject oldOnTileObject, LeverClass oldLever, TeleportClass oldTeleport)
    {
        var lever = tile.Lever;
        var teleport = tile.Teleport;
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
            case OnTileObject.WaterLily:
                tile.WaterLily = true;
                break;
            case OnTileObject.Teleport:
                tile.Teleport = oldTeleport;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
            
        Drawer.Redo.Push(() => PlaceObject(tile, onTileObject, lever, teleport));
    }
}
