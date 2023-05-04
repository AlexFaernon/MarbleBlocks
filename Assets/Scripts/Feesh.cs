using System;
using System.Collections.Generic;
using UnityEngine;

public class Feesh : MonoBehaviour
{
    public bool isActive;
    private Tile _currentTile;
    public Vector2Int GetSave => _currentTile.gridPosition;
    
    private void Update()
    {
        if (!isActive || _currentTile.isJumperOnTile || _currentTile.isSonicOnTile)
        {
            return;
        }

        if (!Input.GetMouseButtonDown(0)) return;
        
        var layerObject = LayerMask.GetMask("Ground");
        var ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        var hit = Physics2D.Raycast(ray, ray, Mathf.Infinity, layerObject);
        if (hit.collider != null)
        {
            var targetTile = hit.collider.gameObject.GetComponent<Tile>();
            if (CanSwimToTile(targetTile))
            {
                transform.position = targetTile.transform.position;
            }
        }
    }

    private bool CanSwimToTile(Tile targetTile)
    {
        var queue = new Queue<Tile>();
        var visited = new HashSet<Tile>();
        
        queue.Enqueue(_currentTile);
        
        while (queue.Count > 0)
        {
            var currentTile = queue.Dequeue();
            visited.Add(currentTile);

            if (currentTile == targetTile)
            {
                return true;
            }
            
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

        return false;
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
        }
        
        if (col.CompareTag("Lever"))
        {
            col.GetComponent<Lever>().Switch();
        }
    }
}
