using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteHelpInEditor : MonoBehaviour
{
    private static Queue<(Vector2Int, Side)> _sonicQueue;
    private static Queue<(Vector2Int, Side)> _jumperQueue;
    private static Queue<Vector2Int> _feeshQueue;

    private void Awake()
    {
        ResetHelp();
    }
    
    public static HelpClass GetHelp()
    {
        var help = new HelpClass();
        if (_jumperQueue.Count > 0)
        {
            help.JumperPos = _jumperQueue.Peek().Item1;
            help.JumperDirection = _jumperQueue.Peek().Item2;
        }
        if (_sonicQueue.Count > 0)
        {
            help.SonicPos = _sonicQueue.Peek().Item1;
            help.SonicDirection = _sonicQueue.Peek().Item2;
        }
        if (_feeshQueue.Count > 0)
        {
            help.FeeshPos = _feeshQueue.Peek();
        }

        return help;
    }
    
    public static void PushSonicMove(Vector2Int pos, Side direction)
    {
        _sonicQueue.Enqueue((pos, direction));
        if (_sonicQueue.Count > 2)
        {
            _sonicQueue.Dequeue();
        }
    }
    
    public static void PushJumperMove(Vector2Int pos, Side direction)
    {
        _jumperQueue.Enqueue((pos, direction));
        if (_jumperQueue.Count > 2)
        {
            _jumperQueue.Dequeue();
        }
    }
    
    public static void PushFeeshMove(Vector2Int pos)
    {
        _feeshQueue.Enqueue(pos);
        if (_feeshQueue.Count > 2)
        {
            _feeshQueue.Dequeue();
        }
    }

    public static void ResetHelp()
    {
        _sonicQueue = new();
        _jumperQueue = new();
        _feeshQueue = new();
    }
}
