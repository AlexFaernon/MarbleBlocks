using Lean.Touch;
using System;
using UnityEngine;

public class MovingManager : MonoBehaviour
{
    public void Up(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;

        MoveCharacterBySwipe(Side.North);
        Debug.Log("Up");
    }

    public void Down(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;

        MoveCharacterBySwipe(Side.South);
        Debug.Log("Dowm");
    }

    public void Left(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;

        MoveCharacterBySwipe(Side.West);
        Debug.Log("Left");
    }

    public void Right(LeanFinger leanFinger)
    {
        if (LeanTouch.Fingers.Count != 1) return;

        MoveCharacterBySwipe(Side.East);
        Debug.Log("Right");
    }

    private void MoveCharacterBySwipe(Side side)
    {
        var sonic = CharacterManager.Sonic;
        if (sonic && sonic.IsActive)
        {
            sonic.StartMovingBySwipe(side);
        }

        var jumper = CharacterManager.Jumper;
        if (jumper && jumper.IsActive)
        {
            jumper.StartMovingBySwipe(side);
        }
    }

    private void Update()
    {
        Vector2 ray;
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touch = Input.GetTouch(0);
            ray = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            return;
        }

        var layerObject = LayerMask.GetMask("Ground", "UI");

        var hit = Physics2D.Raycast(ray, Vector2.zero, Mathf.Infinity, layerObject);
        if (hit.collider is null) return;

        var targetTile = hit.collider.gameObject.GetComponent<Tile>();
        var sonic = CharacterManager.Sonic;
        if (sonic && sonic.IsActive)
        {
            sonic.StartMovingByTileClick(targetTile);
        }

        var jumper = CharacterManager.Jumper;
        if (jumper && jumper.IsActive)
        {
            jumper.StartMovingByTileClick(targetTile);
        }

        var feesh = CharacterManager.Feesh;
        if (feesh && feesh.IsActive)
        {
            feesh.StartMoving(targetTile);
        }
    }
}
