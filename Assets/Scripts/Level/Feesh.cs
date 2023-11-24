using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class Feesh : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float speed;
    public CharacterSwitchButton switchButton;
    private bool _isActive;
    public Tile CurrentTile { get; private set; }
    private HashSet<Tile> _availableTiles;
    private Dictionary<Tile, Tile> _paths;
    private Stack<Tile> _currentPath;
    private Collider2D _collider;
    public Vector2Int GetGridPosition => CurrentTile.gridPosition;
    public bool IsMoving { get; private set; }
    public static int Count;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            if (value)
            {
                FindAvailableTiles();
                TileManager.HighlightTiles(_availableTiles);
            }
        }
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        Count++;
        CharacterManager.Feesh = this;
    }

    private void Update()
    {
        if (!IsActive || CurrentTile.isJumperOnTile || CurrentTile.isSonicOnTile || IsMoving)
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
        if (hit.collider is not null)
        {
            var targetTile = hit.collider.gameObject.GetComponent<Tile>();
            if (_availableTiles.Contains(targetTile))
            {
                _currentPath = GetPathToTile(targetTile);
                IsMoving = true;
                _collider.enabled = false;
                StartCoroutine(Move());
            }
        }
    }

    public void Reset()
    {
        IsMoving = false;
        IsActive = false;
        StopAllCoroutines();
        _collider.enabled = true;
    }

    private IEnumerator Move()
    {
        var currentTile = CurrentTile;
        Debug.Log($"Start {currentTile.gridPosition}");
        _currentPath.Pop();
        while (_currentPath.Count != 0)
        {
            var nextTile = _currentPath.Pop();
            while ((transform.position - nextTile.transform.position).magnitude > 0.1f)
            {
                transform.up = nextTile.transform.position - transform.position;
                transform.Translate(Time.deltaTime * speed * Vector3.up);
                yield return new WaitForEndOfFrame();
            }
            transform.position = nextTile.transform.position;
            currentTile = nextTile;
        }
        _collider.enabled = true;
        IsMoving = false;
        transform.up = Vector3.up;
        CurrentTile = currentTile;
        StepCounter.Count++;
    }

    private void FindAvailableTiles()
    {
        var queue = new Queue<Tile>();
        var visited = new HashSet<Tile>();
        _paths = new();
        
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
                    _paths.TryAdd(nextTile, currentTile);
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
               !nextTile.IsGrass && !nextTile.Whirlpool && !nextTile.IsEdge && !nextTile.WaterLily;
    }

    private Stack<Tile> GetPathToTile(Tile targetTile)
    {
        Assert.IsTrue(_availableTiles.Contains(targetTile));

        var path = new Stack<Tile>();
        var currentTile = targetTile;
        while (currentTile != CurrentTile)
        {
            Debug.Log(currentTile.gridPosition);
            path.Push(currentTile);
            currentTile = _paths[currentTile];
        }
        path.Push(currentTile);
        return path;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelSaveManager.LevelNumber is 2 or 3) return;
        if (CurrentTile.isSonicOnTile || CurrentTile.isJumperOnTile) return;
        
        switchButton.ActivateCharacter();
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

    private void OnDestroy()
    {
        Count--;
        CharacterManager.Feesh = null;
    }
}
