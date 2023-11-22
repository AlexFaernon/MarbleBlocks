﻿using System;
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
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override bool Draw(Tile tile)
	{
        if (!Button.interactable) return false;
        
        if (Check(tile, selectedOnTileObject))
        {
            PlaceObject(tile, selectedOnTileObject, null);
            return true;
        }

        return false;
    }
	
	private void PlaceObject(Tile tile, OnTileObject onTileObject, LeverClass lever)
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
    
    private void RevertObject(Tile tile, OnTileObject oldOnTileObject, LeverClass oldLever)
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
}
