using System;
using Lean.Touch;
using UnityEngine;

public class Sonic : MonoBehaviour
{
    private Tile _currentTile;
    private bool isMoving;
    private Side _movingSide;
    private Lever lever;
    public Vector2Int GetSave => _currentTile.gridPosition;

    public bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            if (isMoving || lever is null) return;
            
            if ((transform.position - lever.transform.position).magnitude < 0.01f)
            {
                lever.Switch();
            }

            lever = null;
        }
    }

    private void Awake()
    {
        LeanTouch.OnFingerSwipe += Swipe;
    }

    private void Update()
    {
        if (!_currentTile.isGrass && !_currentTile.isFeeshOnTile && !IsMoving)
        {
            Debug.Log("Drown");
            Destroy(gameObject);
            return;
        }

        if (_currentTile.isEdge)
        {
            Debug.Log("Fall");
            Destroy(gameObject);
            return;
        }

        if (_currentTile.isJumperOnTile)
        {
            return;
        }
        
        if (!IsMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                IsMoving = true;
                _movingSide = Side.North;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                IsMoving = true;
                _movingSide = Side.South;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                IsMoving = true;
                _movingSide = Side.East;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                IsMoving = true;
                _movingSide = Side.West;
            }
            
            return;
        }

        var enterSide = _movingSide switch
        {
            Side.North => Side.South,
            Side.South => Side.North,
            Side.West => Side.East,
            Side.East => Side.West,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var nextTile = TileManager.GetTile(_currentTile, _movingSide);

        if (_currentTile.AvailableToMoveThroughSide(_movingSide) && nextTile.AvailableToMoveThroughSide(enterSide) &&
            !nextTile.isJumperOnTile)
        {
            transform.position = nextTile.transform.position;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void Swipe(LeanFinger leanFinger)
    {
        //Debug.Log();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            _currentTile = col.GetComponent<Tile>();
        }
        
        if (col.CompareTag("Spike"))
        {
            Destroy(gameObject);
        }

        if (col.CompareTag("Lever"))
        {
            lever = col.GetComponent<Lever>();
        }
    }
}
