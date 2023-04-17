using System;
using UnityEngine;

public class Sonic : MonoBehaviour
{
    private Tile _currentTile;
    private bool _isMoving;
    private Side _movingSide;

    private void Update()
    {
        if (!_currentTile.isGrass && !_currentTile.isFeeshOnTile && !_isMoving)
        {
            Destroy(gameObject);
        }

        if (_currentTile.isJumperOnTile)
        {
            return;
        }
        
        if (!_isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _isMoving = true;
                _movingSide = Side.North;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _isMoving = true;
                _movingSide = Side.South;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _isMoving = true;
                _movingSide = Side.East;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _isMoving = true;
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
        if (!nextTile)
        {
            Debug.Log("Out of map");
            _isMoving = false;
            return;
        }
        
        if (_currentTile.AvailableToMoveThroughSide(_movingSide) && nextTile.AvailableToMoveThroughSide(enterSide) &&
            !nextTile.isJumperOnTile)
        {
            transform.position = nextTile.transform.position;
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
