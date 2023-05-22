using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    private Sonic sonic;
    private Jumper jumper;
    
    private void Awake()
    {
        var sonicObj = GameObject.FindWithTag("Sonic");
        if (sonicObj)
        {
            sonic = sonicObj.GetComponent<Sonic>();
        }
        var jumperObj = GameObject.FindWithTag("Jumper");
        if (jumperObj)
        {
            jumper = jumperObj.GetComponent<Jumper>();
        }
    }
    
    public void Up(LeanFinger leanFinger)
    {
        MoveCharacter(Side.North);
        Debug.Log("Up");
    }
    
    public void Down(LeanFinger leanFinger)
    {
        MoveCharacter(Side.South);
        Debug.Log("Dowm");
    }
    
    public void Left(LeanFinger leanFinger)
    {
        MoveCharacter(Side.West);
        Debug.Log("Left");
    }
    
    public void Right(LeanFinger leanFinger)
    {
        MoveCharacter(Side.East);
        Debug.Log("Right");
    }

    private void MoveCharacter(Side side)
    {
        if (sonic && sonic.IsActive)
        {
            sonic.Move(side);
        }
        if (jumper && jumper.IsActive)
        {
            jumper.Move(side);
        }
    }
}
