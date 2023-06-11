using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class Jumper : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float speed;
    public Tile CurrentTile { get; private set; }
    private Side _movingSide;
    private bool _isActive;
    private Feesh _feesh;
    private Sonic _sonic;
    private Collider2D _collider2D;
    public Vector2Int GetGridPosition => CurrentTile.gridPosition;
    [SerializeField] private Animator animator;
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
        _collider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!CurrentTile.IsGrass && !CurrentTile.isFeeshOnTile && !IsMoving || CurrentTile.IsEdge)
        {
            GameObject.FindWithTag("Defeat").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private IEnumerator Move(Tile targetTile)
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
        _collider2D.enabled = false;
        while ((transform.position - targetTile.transform.position).magnitude > 0.1f)
        {
            transform.Translate(Time.deltaTime * speed * movingVector);

            yield return new WaitForEndOfFrame();
        }
        animator.ResetTrigger("RIGHT");
        animator.ResetTrigger("UP");
        animator.ResetTrigger("DOWN");
        animator.ResetTrigger("LEFT");
        transform.position = targetTile.transform.position;

        StepCounter.Count++;
        IsMoving = false;
        _collider2D.enabled = true;
    }

    public void StartMoving(Side side)
    {
        Assert.IsTrue(IsActive);
        
        if (IsMoving) return;
        
        _movingSide = side;
        var targetTile = GetTargetTile(_movingSide);
        if (targetTile != CurrentTile)
        {
            IsMoving = true;
            StartCoroutine(Move(targetTile));
        }
    }
    
    private Tile GetTargetTile(Side movingSide)
    {
        var enterSide = movingSide switch
        {
            Side.North => Side.South,
            Side.South => Side.North,
            Side.West => Side.East,
            Side.East => Side.West,
            _ => throw new ArgumentOutOfRangeException()
        };

        var targetTile = CurrentTile;
        for (var i = 0; i < 2; i++)
        {
            var nextTile = TileManager.GetTile(targetTile, movingSide);
            if (!nextTile)
            {
                return targetTile;
            }

            if (targetTile.AvailableToMoveThroughSide(movingSide) && nextTile.AvailableToMoveThroughSide(enterSide) && !targetTile.IsExitOnTile)
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
            var targetTile = GetTargetTile(side);
            if (targetTile != CurrentTile && !targetTile.IsEdge)
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
            CurrentTile = col.GetComponent<Tile>();
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
