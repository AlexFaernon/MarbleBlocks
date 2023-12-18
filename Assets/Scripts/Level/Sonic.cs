using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class Sonic : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float speed;
    public Tile CurrentTile { get; private set; }
    public CharacterSwitchButton switchButton;
    private bool _isMoving;
    private Side _movingSide;
    private Lever _lever;
    public bool isActive;
    public Vector2Int GetGridPosition => CurrentTile.gridPosition;
    [SerializeField] private Animator animator;
    public static int Count;

    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            if (_isMoving)
            {
                CurrentTile.WaterLily = false;
            }
            
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

    private void Awake()
    {
        Count++;
        CharacterManager.Sonic = this;
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

    public void Reset()
    {
        IsActive = false;
        IsMoving = false;
        StopAllCoroutines();
    }

    private void Update()
    {
        if (!CurrentTile.IsGrass && !CurrentTile.WaterLily && !CurrentTile.isFeeshOnTile && !IsMoving || CurrentTile.IsEdge)
        {
            WinLoseManager.Lose.SetActive(true);
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
                while ((transform.position - nextTile.transform.position).magnitude > 0.2f)
                {
                    var direction = nextTile.transform.position - transform.position;
                    transform.Translate(Time.deltaTime * speed * direction.normalized);
                    yield return new WaitForEndOfFrame();
                }

                transform.position = nextTile.transform.position;
            }
            else
            {
                animator.ResetTrigger("RIGHT");
                animator.ResetTrigger("UP");
                animator.ResetTrigger("DOWN");
                animator.ResetTrigger("LEFT");
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
                if (currentTile.IsEdge)
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
        if (CurrentTile.isJumperOnTile) return;
        
        switchButton.ActivateCharacter();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            CurrentTile = col.GetComponent<Tile>();
        }
        
        if (col.CompareTag("Spike"))
        {
            WinLoseManager.Lose.SetActive(true);
        }

        if (col.CompareTag("Lever"))
        {
            _lever = col.GetComponent<Lever>();
        }
    }

    private void OnDestroy()
    {
        Count--;
        CharacterManager.Sonic = null;
    }
}
