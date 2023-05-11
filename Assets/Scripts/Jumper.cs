using System;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Tile _currentTile;
    private Side _movingSide;
    public bool isActive;
    public Vector2Int GetSave => _currentTile.gridPosition;

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

    public bool IsMoving { get; set; }

    void Update()
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
        
         var targetTile = TryToMove(_movingSide);
         if (targetTile != _currentTile)
         {
             transform.position = targetTile.transform.position;
         }
         IsMoving = false;
    }

    public void Move(Side side)
    {
        if (!IsActive)
            throw new Exception("Character isn't active");

        IsMoving = true;
        _movingSide = side;
    }
    
    private Tile TryToMove(Side movingSide)
    {
        var enterSide = movingSide switch
        {
            Side.North => Side.South,
            Side.South => Side.North,
            Side.West => Side.East,
            Side.East => Side.West,
            _ => throw new ArgumentOutOfRangeException()
        };

        var targetTile = _currentTile;
        for (var i = 0; i < 2; i++)
        {
            var nextTile = TileManager.GetTile(targetTile, movingSide);
            if (!nextTile)
            {
                return targetTile;
            }

            if (targetTile.AvailableToMoveThroughSide(movingSide) && nextTile.AvailableToMoveThroughSide(enterSide))
            {
                targetTile = nextTile;
            }
            else
            {
                return targetTile;
            }
        }

        return targetTile;
    }

    private void HighlightTiles()
    {
        var highlightedTiles = new HashSet<Tile>();
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            var targetTile = TryToMove(side);
            if (targetTile != _currentTile)
            {
                highlightedTiles.Add(targetTile);
            }
        }
        
        TileManager.HighlightTiles(highlightedTiles);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            _currentTile = col.GetComponent<Tile>();
            HighlightTiles();
        }
        
        if (col.CompareTag("Spike"))
        {
            Destroy(gameObject);
        }
        
        if (col.CompareTag("Lever"))
        {
            col.GetComponent<Lever>().Switch();
        }
    }
}
