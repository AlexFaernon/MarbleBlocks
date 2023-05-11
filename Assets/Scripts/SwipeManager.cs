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
        sonic = GameObject.FindWithTag("Sonic").GetComponent<Sonic>();
        jumper = GameObject.FindWithTag("Jumper").GetComponent<Jumper>();
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
        if (sonic.IsActive)
        {
            sonic.Move(side);
        }
        if (jumper.IsActive)
        {
            jumper.Move(side);
        }
    }
}
