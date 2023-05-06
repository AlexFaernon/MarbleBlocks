using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class Sonic : MonoBehaviour
{
    private Tile _currentTile;
    private bool isMoving;
    private Side _movingSide;
    private Lever lever;
    public bool isActive;
    public Vector2Int GetSave => _currentTile.gridPosition;

    public bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            if (isMoving) return;
            
            if (IsActive)
            {
                HighlightTiles();
            }
            
            if (lever is not null && (transform.position - lever.transform.position).magnitude < 0.01f)
            {
                lever.Switch();
            }
            lever = null;
        }
    }

    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            if (value)
            {
                HighlightTiles();
            }
        }
    }

    public void Move(Side side)
    {
        if (!IsActive)
            throw new Exception("Character isn't active");
        
        if (IsMoving || _currentTile.isJumperOnTile) return;
        
        IsMoving = true;
        _movingSide = side;
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
        
        if (!IsMoving) return;

        var nextTile = TileManager.GetTile(_currentTile, _movingSide);

        if (CanMoveForward(_currentTile, _movingSide))
        {
            transform.position = nextTile.transform.position;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void HighlightTiles()
    {
        var highlightedTiles = new HashSet<Tile>();
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            var currentTile = _currentTile;
            while (CanMoveForward(currentTile, side))
            {
                currentTile = TileManager.GetTile(currentTile, side);
                if (currentTile.isEdge)
                {
                    break;
                }
                highlightedTiles.Add(currentTile);
            }
        }
        
        TileManager.HighlightTiles(highlightedTiles);
    }

    private bool CanMoveForward(Tile currentTile, Side moveSide)
    {
        var nextTile = TileManager.GetTile(_currentTile, moveSide);
        
        var enterSide = _movingSide switch
        {
            Side.North => Side.South,
            Side.South => Side.North,
            Side.West => Side.East,
            Side.East => Side.West,
            _ => throw new ArgumentOutOfRangeException()
        };

        return currentTile.AvailableToMoveThroughSide(moveSide) && nextTile.AvailableToMoveThroughSide(enterSide) &&
               !nextTile.isJumperOnTile;
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
