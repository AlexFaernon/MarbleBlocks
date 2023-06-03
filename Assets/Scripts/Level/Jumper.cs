using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jumper : MonoBehaviour, IPointerDownHandler
{
    private Tile _currentTile;
    private Side _movingSide;
    private bool _isActive;
    private Feesh _feesh;
    private Sonic _sonic;
    public Vector2Int GetGridPosition => _currentTile.gridPosition;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            if (value)
            {
                HighlightTiles();
            }
        }
    }

    public bool IsMoving { get; set; }
    
    private void Start()
    {
        _feesh = GameObject.FindWithTag("Feesh")?.GetComponent<Feesh>();
        _sonic = GameObject.FindWithTag("Sonic")?.GetComponent<Sonic>();
    }

    void Update()
    {
        if (!_currentTile.isGrass && !_currentTile.isFeeshOnTile && !IsMoving)
        {
            Debug.Log("Drown");
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
            return;
        }
        
        if (_currentTile.isEdge)
        {
            Debug.Log("Fall");
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
            return;
        }

        if (!IsMoving) return;
        
         var targetTile = TryToMove(_movingSide);
         if (targetTile != _currentTile)
         {
             transform.position = targetTile.transform.position;
             StepCounter.Count++;
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
            if (targetTile != _currentTile && !targetTile.isEdge)
            {
                highlightedTiles.Add(targetTile);
            }
        }
        
        TileManager.HighlightTiles(highlightedTiles);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelSaveManager.LevelNumber == 2) return;
        
        IsActive = true;
        if (_feesh) _feesh.IsActive = false;
        if (_sonic) _sonic.IsActive = false;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            _currentTile = col.GetComponent<Tile>();
            if (IsActive)
            {
                HighlightTiles();
            }
        }
        
        if (col.CompareTag("Spike"))
        {
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }
        
        if (col.CompareTag("Lever"))
        {
            col.GetComponent<Lever>().Switch();
        }
    }
}
