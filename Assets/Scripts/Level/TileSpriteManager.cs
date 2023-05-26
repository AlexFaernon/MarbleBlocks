using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TileSpriteManager : MonoBehaviour
{
    private static Sprite[] _groundMid;
    private static Sprite[] _groundBot;
    private static Sprite[] _waterMib;
    private static Sprite[] _waterBot;
    private static readonly Dictionary<(DoorLeverColor, bool), Sprite> Doors = new();
    private static readonly Dictionary<(DoorLeverColor, bool), Sprite> Levers = new();
    private static readonly Random Random = new();

    public static Sprite GetRandomGroundMid => GetRandomSprite(_groundMid);
    public static Sprite GetRandomGroundBot => GetRandomSprite(_groundBot);
    public static Sprite GetRandomWaterMid => GetRandomSprite(_waterMib);
    public static Sprite GetRandomWaterBot => GetRandomSprite(_waterBot);

    private void Awake()
    {
        _groundMid = Resources.LoadAll<Sprite>("Tiles\\Land\\Mid");
        _groundBot = Resources.LoadAll<Sprite>("Tiles\\Land\\Bottom");
        _waterMib = Resources.LoadAll<Sprite>("Tiles\\Water\\Mid");
        _waterBot = Resources.LoadAll<Sprite>("Tiles\\Water\\Bottom");
    }

    public static Sprite GetLeverSprite(DoorLeverColor color, bool isOn)
    {
        if (!Levers.ContainsKey((color, isOn)))
        {
            Levers[(color, false)] = Resources.Load<Sprite>($"Levers\\Off\\{color}");
            Levers[(color, true)] = Resources.Load<Sprite>($"Levers\\On\\{color}");
        }
        
        return Levers[(color, isOn)];
    }

    public static Sprite GetDoorSprite(DoorLeverColor color, bool isOpened)
    {
        if (!Doors.ContainsKey((color, isOpened)))
        {
            Doors[(color, false)] = Resources.Load<Sprite>($"Doors\\Closed\\{color}");
            Doors[(color, true)] = Resources.Load<Sprite>($"Doors\\Opened\\{color}");
        }

        return Doors[(color, isOpened)];
    }

    private static Sprite GetRandomSprite(Sprite[] sprites)
    {
        return sprites[Random.Next(sprites.Length)];
    }
}