using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Feesh : MonoBehaviour, IPointerDownHandler
{
    private bool isActive;
    private Sonic _sonic;
    private Jumper _jumper;
    public Tile CurrentTile { get; private set; }
    private HashSet<Tile> _availableTiles = new();
    public Vector2Int GetSave => CurrentTile.gridPosition;

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

    private void Start()
    {
        _sonic = GameObject.FindWithTag("Sonic")?.GetComponent<Sonic>();
        _jumper = GameObject.FindWithTag("Jumper")?.GetComponent<Jumper>();
    }

    private void Update()
    {
        if (!IsActive || CurrentTile.isJumperOnTile || CurrentTile.isSonicOnTile)
        {
            return;
        }

        Vector2 ray;
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touch = Input.GetTouch(0);
            ray = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            return;
        }

        var layerObject = LayerMask.GetMask("Ground", "UI");
        
        var hit = Physics2D.Raycast(ray, Vector2.zero, Mathf.Infinity, layerObject);
        if (hit.collider != null)
        {
            var targetTile = hit.collider.gameObject.GetComponent<Tile>();
            if (_availableTiles.Contains(targetTile) && targetTile != CurrentTile)
            {
                transform.position = targetTile.transform.position;
                StepCounter.Count++;
            }
        }
    }

    private void FindAvailableTiles()
    {
        var queue = new Queue<Tile>();
        var visited = new HashSet<Tile>();
        
        queue.Enqueue(CurrentTile);
        
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
        _availableTiles.Remove(CurrentTile);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelSaveManager.LevelNumber is 2 or 3) return;
        
        IsActive = true;
        if (_sonic) _sonic.IsActive = false;
        if (_jumper) _jumper.IsActive = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            CurrentTile = col.GetComponent<Tile>();
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
