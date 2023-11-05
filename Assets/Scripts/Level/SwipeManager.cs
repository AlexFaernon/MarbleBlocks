using Lean.Touch;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    private Sonic _sonic;
    private Jumper _jumper;
    
    private void OnEnable()
    {
        var sonicObj = GameObject.FindWithTag("Sonic");
        if (sonicObj)
        {
            _sonic = sonicObj.GetComponent<Sonic>();
        }
        var jumperObj = GameObject.FindWithTag("Jumper");
        if (jumperObj)
        {
            _jumper = jumperObj.GetComponent<Jumper>();
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
        if (_sonic && _sonic.IsActive)
        {
            _sonic.StartMoving(side);
        }
        if (_jumper && _jumper.IsActive)
        {
            _jumper.StartMoving(side);
        }
    }
}
