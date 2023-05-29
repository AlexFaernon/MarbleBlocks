using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sonic : MonoBehaviour
{
    private Tile _currentTile;
    private bool _isMoving;
    private Side _movingSide;
    private Lever _lever;
    public bool isActive;
    private Feesh _feesh;
    private Jumper _jumper;
    public Vector2Int GetSave => _currentTile.gridPosition;

    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            _isMoving = value;
            if (_isMoving) return;
            
            if (IsActive)
            {
                HighlightTiles();
            }
            
            if (_lever is not null && (transform.position - _lever.transform.position).magnitude < 0.01f)
            {
                _lever.Switch();
            }
            _lever = null;
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

    private void Start()
    {
        _feesh = GameObject.FindWithTag("Feesh")?.GetComponent<Feesh>();
        _jumper = GameObject.FindWithTag("Jumper")?.GetComponent<Jumper>();
    }

    public void Move(Side side)
    {
        if (!IsActive)
            throw new Exception("Character isn't active");
        
        if (IsMoving || _currentTile.isJumperOnTile) return;

        if (CanMoveForward(_currentTile, side))
        {
            IsMoving = true;
            _movingSide = side;
            StepCounter.Count++;
        }
    }

    private void Update()
    {
        if (!_currentTile.isGrass && !_currentTile.isFeeshOnTile && !IsMoving)
        {
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Debug.Log("Drown");
            Destroy(gameObject);
            return;
        }

        if (_currentTile.isEdge)
        {
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
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
    
    private void OnMouseDown()
    {
        IsActive = true;
        if (_feesh) _feesh.IsActive = false;
        if (_jumper) _jumper.IsActive = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            _currentTile = col.GetComponent<Tile>();
        }
        
        if (col.CompareTag("Spike"))
        {
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }

        if (col.CompareTag("Lever"))
        {
            _lever = col.GetComponent<Lever>();
        }
    }
}
