using Lean.Touch;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public void Up(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;
        MoveCharacter(Side.North);
        Debug.Log("Up");
    }
    
    public void Down(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;
        MoveCharacter(Side.South);
        Debug.Log("Dowm");
    }
    
    public void Left(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;
        MoveCharacter(Side.West);
        Debug.Log("Left");
    }
    
    public void Right(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;
        MoveCharacter(Side.East);
        Debug.Log("Right");
    }

    private void MoveCharacter(Side side)
    {
        var sonic = CharacterManager.Sonic;
        if (sonic && sonic.IsActive)
        {
            sonic.StartMoving(side);
        }
        
        var jumper = CharacterManager.Jumper;
        if (jumper && jumper.IsActive)
        {
            jumper.StartMoving(side);
        }
    }
}
