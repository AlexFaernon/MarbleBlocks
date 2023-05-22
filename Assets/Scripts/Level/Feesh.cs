using System;
using System.Collections.Generic;
using UnityEngine;

public class Feesh : MonoBehaviour
{
    public bool isActive;
    private Tile _currentTile;
    private HashSet<Tile> _availableTiles = new();
    public Vector2Int GetSave => _currentTile.gridPosition;

    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            if (value)
            {
                FindAvailableTiles();
                TileManager.HighlightTiles(_availableTiles);
            }
        }
    }

    private void Update()
    {
        if (!IsActive || _currentTile.isJumperOnTile || _currentTile.isSonicOnTile)
        {
            return;
        }

        Vector2 ray;
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            ray = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            return;
        }

        var layerObject = LayerMask.GetMask("Ground");
        
        var hit = Physics2D.Raycast(ray, ray, Mathf.Infinity, layerObject);
        if (hit.collider != null)
        {
            var targetTile = hit.collider.gameObject.GetComponent<Tile>();
            if (_availableTiles.Contains(targetTile))
            {
                transform.position = targetTile.transform.position;
            }
        }
    }

    private void FindAvailableTiles()
    {
        var queue = new Queue<Tile>();
        var visited = new HashSet<Tile>();
        
        queue.Enqueue(_currentTile);
        
        while (queue.Count > 0)
        {
            var currentTile = queue.Dequeue();
            visited.Add(currentTile);

            foreach (Side side in Enum.GetValues(typeof(Side)))
            {
                var nextTile = TileManager.GetTile(currentTile, side);
                if (!nextTile) continue;

                if (!visited.Contains(nextTile) && !queue.Contains(nextTile) && TileIsAvailable(currentTile, side))
                {
                    queue.Enqueue(nextTile);
                }
            }
        }

        _availableTiles = visited;
    }

    private bool TileIsAvailable(Tile tile, Side movingSide)
    {
        var enterSide = movingSide switch
        {
            Side.North => Side.South,
            Side.South => Side.North,
            Side.West => Side.East,
            Side.East => Side.West,
            _ => throw new ArgumentOutOfRangeException()
        };

        var nextTile = TileManager.GetTile(tile, movingSide);

        if (!nextTile)
        {
            return false;
        }
        
        return tile.AvailableToMoveThroughSide(movingSide) && nextTile.AvailableToMoveThroughSide(enterSide) &&
               !nextTile.isGrass && !nextTile.IsWhirlpoolOnTile && !nextTile.isEdge;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            _currentTile = col.GetComponent<Tile>();
            if (IsActive)
            {
                FindAvailableTiles();
                TileManager.HighlightTiles(_availableTiles);
            }
        }
        
        if (col.CompareTag("Lever"))
        {
            col.GetComponent<Lever>().Switch();
            if (IsActive)
            {
                FindAvailableTiles();
                TileManager.HighlightTiles(_availableTiles);
            }
        }
    }
}
