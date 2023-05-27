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

    private static Sprite GetRandomSprite(Sprite[] sprites)
    {
        return sprites[Random.Next(sprites.Length)];
    }
}