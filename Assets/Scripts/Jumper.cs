using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    private Tile _currentTile;
    private Tile _targetTile;
    private Side _movingSide;
    private bool _isMoving;
    
    void Update()
    {
        if (!_currentTile.isGrass && !_currentTile.isFeeshOnTile && !_isMoving)
        {
            Destroy(gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            _isMoving = true;
            _movingSide = Side.North;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _isMoving = true;
            _movingSide = Side.South;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _isMoving = true;
            _movingSide = Side.East;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _isMoving = true;
            _movingSide = Side.West;
        }

        if (!_isMoving) return;

        _targetTile = _currentTile;
        TryToMove();
        TryToMove();
        _isMoving = false;
        transform.position = _targetTile.transform.position;
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
            Debug.Log("Out of map");
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
    }
}
