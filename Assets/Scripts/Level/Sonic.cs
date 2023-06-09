using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Sonic : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float speed;
    public Tile CurrentTile { get; private set; }
    private bool _isMoving;
    private Side _movingSide;
    private Lever _lever;
    public bool isActive;
    private Feesh _feesh;
    private Jumper _jumper;
    public Vector2Int GetSave => CurrentTile.gridPosition;
    [SerializeField] private Animator animator;

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
            
            if (_lever is not null && (transform.position - _lever.transform.position).magnitude < 0.1f)
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

    public void StartMoving(Side side)
    {
        Assert.IsTrue(IsActive);
        
        if (IsMoving || CurrentTile.isJumperOnTile) return;

        if (CanMoveForward(CurrentTile, side))
        {
            IsMoving = true;
            _movingSide = side;
            StartCoroutine(Move());
        }
    }

    private void Update()
    {
        if (!CurrentTile.isGrass && !CurrentTile.isFeeshOnTile && !IsMoving || CurrentTile.isEdge)
        {
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private IEnumerator Move()
    {
        var movingVector = _movingSide switch
        {
            Side.North => Vector2.up,
            Side.South => Vector2.down,
            Side.West => Vector2.left,
            Side.East => Vector2.right,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (movingVector == Vector2.right)
            animator.SetTrigger("RIGHT");
        if (movingVector == Vector2.up)
            animator.SetTrigger("UP");
        if (movingVector == Vector2.down)
            animator.SetTrigger("DOWN");
        if (movingVector == Vector2.left)
            animator.SetTrigger("LEFT");
        while (IsMoving)
        {
            var nextTile = TileManager.GetTile(CurrentTile, _movingSide);
            if (CanMoveForward(CurrentTile, _movingSide))
            {
                while ((transform.position - nextTile.transform.position).magnitude > 0.1f)
                {
                    transform.Translate(Time.deltaTime * speed * movingVector);
                    yield return new WaitForEndOfFrame();
                }

                transform.position = nextTile.transform.position;
                animator.ResetTrigger("RIGHT");
                animator.ResetTrigger("UP");
                animator.ResetTrigger("DOWN");
                animator.ResetTrigger("LEFT");
            }
            else
            {
                IsMoving = false;
            }
        }
        StepCounter.Count++;
    }

    private void HighlightTiles()
    {
        var highlightedTiles = new HashSet<Tile>();
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            var currentTile = CurrentTile;
            while (CanMoveForward(currentTile, side))
            {
                currentTile = TileManager.GetTile(currentTile, side);
                if (currentTile.isEdge)
                {
                    break;
                }
            }
            if (currentTile != CurrentTile)
            {
                highlightedTiles.Add(currentTile);
            }
        }
        
        TileManager.HighlightTiles(highlightedTiles);
    }

    private bool CanMoveForward(Tile currentTile, Side moveSide)
    {
        var nextTile = TileManager.GetTile(currentTile, moveSide);
        
        var enterSide = moveSide switch
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
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelSaveManager.LevelNumber == 3) return;
        
        IsActive = true;
        if (_feesh) _feesh.IsActive = false;
        if (_jumper) _jumper.IsActive = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            CurrentTile = col.GetComponent<Tile>();
            Debug.Log($"current tile is null? {CurrentTile is null}");
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
