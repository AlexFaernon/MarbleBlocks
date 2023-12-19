using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteHelpInEditor : MonoBehaviour
{
    private static (Vector2Int, Side) _sonicQueue;
    private static (Vector2Int, Side) _jumperQueue;
    private static Vector2Int _feeshQueue;

    private void Awake()
    {
        ResetHelp();
    }
    
    public static HelpClass GetHelp()
    {
        return new HelpClass
        {
            JumperPos = _jumperQueue.Item1,
            JumperDirection = _jumperQueue.Item2,
            SonicPos = _sonicQueue.Item1,
            SonicDirection = _sonicQueue.Item2,
            FeeshPos = _feeshQueue
        };
    }
    
    public static void PushSonicMove(Vector2Int pos, Side direction)
    {
        _sonicQueue = (pos, direction);
    }
    
    public static void PushJumperMove(Vector2Int pos, Side direction)
    {
        _jumperQueue = (pos, direction);
    }
    
    public static void PushFeeshMove(Vector2Int pos)
    {
        _feeshQueue = pos;
    }

    public static void ResetHelp()
    {
        _sonicQueue = new();
        _jumperQueue = new();
        _feeshQueue = new();
    }
}
