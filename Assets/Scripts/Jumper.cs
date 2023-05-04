using System;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Tile _currentTile;
    private Tile _targetTile;
    private Side _movingSide;
    private bool _isMoving;
    public bool isActive;
    public Vector2Int GetSave => _currentTile.gridPosition;
    
    void Update()
    {
        if (!_currentTile.isGrass && !_currentTile.isFeeshOnTile && !_isMoving)
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

        if (!_isMoving) return;

        _targetTile = _currentTile;
        TryToMove();
        TryToMove();
        _isMoving = false;
        transform.position = _targetTile.transform.position;
    }

    public void Move(Side side)
    {
        if (!isActive)
            throw new Exception("Character isn't active");

        _isMoving = true;
        _movingSide = side;
    }
    
    private void TryToMove()
    {
        var enterSide = _movingSide switch
        {
            Side.North => Side.South,
            Side.South => Side.North,
            Side.West => Side.East,
            Side.East => Side.West,
            _ => throw new ArgumentOutOfRangeException()
        };

        var nextTile = TileManager.GetTile(_targetTile, _movingSide);
        if (!nextTile)
        {
            _isMoving = false;
            return;
        }

        if (_targetTile.AvailableToMoveThroughSide(_movingSide) && nextTile.AvailableToMoveThroughSide(enterSide))
        {
            _targetTile = nextTile;
        }
        else
        {
            _isMoving = false;
        }
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
            col.GetComponent<Lever>().Switch();
        }
    }
}
