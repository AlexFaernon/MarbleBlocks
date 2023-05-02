using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    private Sonic sonic;
    
    private void Awake()
    {
        sonic = GameObject.FindWithTag("Sonic").GetComponent<Sonic>();
    }
    
    public void Up(LeanFinger leanFinger)
    {
        sonic.Move(Side.North);
        Debug.Log("Up");
    }
    
    public void Down(LeanFinger leanFinger)
    {
        sonic.Move(Side.South);
        Debug.Log("Dowm");
    }
    
    public void Left(LeanFinger leanFinger)
    {
        sonic.Move(Side.West);
        Debug.Log("Left");
    }
    
    public void Right(LeanFinger leanFinger)
    {
        sonic.Move(Side.East);
        Debug.Log("Right");
    }
}
