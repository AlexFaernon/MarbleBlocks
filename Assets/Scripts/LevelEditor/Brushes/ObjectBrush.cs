using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBrush : Brush
{
    [SerializeField] private TMP_Text limitLabel;
    public OnTileObject selectedOnTileObject;

    private int ObjectLimit
    {
        get
        {
            return selectedOnTileObject switch
            {
                OnTileObject.None => throw new ArgumentOutOfRangeException("None object"),
                OnTileObject.Spike => LevelObjectsLimits.Spike,
                OnTileObject.Whirlpool => LevelObjectsLimits.Whirlpool,
                OnTileObject.Lever => LevelObjectsLimits.Lever,
                OnTileObject.Exit => throw new ArgumentOutOfRangeException("Exit has no limits"),
                OnTileObject.WaterLily => LevelObjectsLimits.WaterLily,
                OnTileObject.Teleport => LevelObjectsLimits.Teleport,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private int ObjectCount
    {
        get
        {
            return selectedOnTileObject switch
            {
                OnTileObject.None => throw new ArgumentOutOfRangeException("None object"),
                OnTileObject.Spike => Spike.Count,
                OnTileObject.Whirlpool => Whirlpool.Count,
                OnTileObject.Lever => Lever.Count,
                OnTileObject.Exit => throw new ArgumentOutOfRangeException("Exit has no limits"),
                OnTileObject.WaterLily => WaterLily.Count,
                OnTileObject.Teleport => Teleport.Count,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        Image = GetComponent<Image>();
        UnselectedSprite = Image.sprite;
        var levelRequirement = selectedOnTileObject switch
        {
            OnTileObject.None or OnTileObject.Spike or OnTileObject.Whirlpool or OnTileObject.Exit => 0,
            OnTileObject.Lever => LevelObjectsLimits.LeverLevel,
            OnTileObject.WaterLily => LevelObjectsLimits.WaterlilyLevel,
            OnTileObject.Teleport => LevelObjectsLimits.TeleportLevel,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (PlayerData.SingleLevelCompleted < levelRequirement)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Image.sprite = Drawer.CurrentBrush == this ? selectedSprite : UnselectedSprite;

        if (selectedOnTileObject != OnTileObject.Exit)
        {
            Button.interactable = ObjectCount < ObjectLimit;
            limitLabel.text = (ObjectLimit - ObjectCount).ToString();
        }

        if (selectedOnTileObject == OnTileObject.Teleport)
        {
            AllowedColors[DoorLeverColor.Red] = false;
            AllowedColors[DoorLeverColor.Grey] = false;
            AllowedColors[DoorLeverColor.Purple] = true;
            AllowedColors[DoorLeverColor.Green] = true;
            AllowedColors[DoorLeverColor.Blue] = true;
            //var notPairedTeleportColor = Teleport.CountByColors.Where(pair => pair.Value == 1).Select(pair => pair.Key);
            
            var pairedTeleportsColors = Teleport.CountByColors.Where(pair => pair.Value == 2).Select(pair => pair.Key);
            foreach (var pairedTeleportsColor in pairedTeleportsColors)
            {
                AllowedColors[pairedTeleportsColor] = false;
            }
        }
    }

    public override bool Draw(Tile tile)
	{
        if (!Button.interactable || !AllowedColors[Color]) return false;
        
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
