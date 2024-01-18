using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Spine.Unity;

public class Jumper : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float speed;
    public Tile CurrentTile { get; private set; }
    public CharacterSwitchButton switchButton;
    private Side _movingSide;
    private bool _isActive;
    private Collider2D _collider2D;
    public Vector2Int GridPosition => CurrentTile.gridPosition;
   // [SerializeField] private Animator animator;
    public static int Count;
    
    private SkeletonAnimationMulti skeletonAnimation;


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

    public bool IsMoving { get; private set; }
    
    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        
        skeletonAnimation = GetComponent<SkeletonAnimationMulti>();
        
        Count++;
        CharacterManager.Jumper = this;
    }

    private IEnumerator Move(Tile targetTile)
    {
        _collider2D.enabled = false;
        var movingVector = _movingSide switch
        {
            Side.North => Vector2.up,
            Side.South => Vector2.down,
            Side.West => Vector2.left,
            Side.East => Vector2.right,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (movingVector == Vector2.right)
        {
            Debug.Log("Jumper right");
            skeletonAnimation.SetAnimation("liz side jump", false);
            skeletonAnimation.CurrentSkeletonAnimation.AnimationState.TimeScale = 2;
            //animator.SetTrigger("RIGHT");
        }        
        if (movingVector == Vector2.up)
        {
            Debug.Log("Jumper up");
            //animator.SetTrigger("UP");
        }        
        if (movingVector == Vector2.down)
        {
            Debug.Log("Jumper down");
            skeletonAnimation.SetAnimation("liz straight jump", false);
            //animator.SetTrigger("DOWN");
        }        
        if (movingVector == Vector2.left)
        {
            Debug.Log("Jumper left");
            //animator.SetTrigger("LEFT");
        }        
        
        while ((transform.position - targetTile.transform.position).magnitude > 0.1f)
        {
            var direction = targetTile.transform.position - transform.position;
            transform.Translate(Time.deltaTime * speed * direction.normalized);
            yield return new WaitForEndOfFrame();
        }
        
        // animator.ResetTrigger("RIGHT");
        // animator.ResetTrigger("UP");
        // animator.ResetTrigger("DOWN");
        // animator.ResetTrigger("LEFT");
        
        transform.position = targetTile.transform.position;
        _collider2D.enabled = true;
        CurrentTile = targetTile;
        IsMoving = false;
        skeletonAnimation.ClearAnimation();
        skeletonAnimation.SetAnimation("liz afk animation", true);
    }

    public void StartMoving(Side side)
    {
        Assert.IsTrue(IsActive);
        
        if (IsMoving) return;
        
        _movingSide = side;
        var targetTile = GetTargetTile(_movingSide);
        if (targetTile != CurrentTile)
        {
            StepCounter.Count++;
            if (GameMode.CurrentGameMode == GameModeType.LevelEditor)
            {
                WriteHelpInEditor.PushJumperMove(GridPosition, side);
            }
            IsMoving = true;
            CurrentTile.WaterLily = false;
            StartCoroutine(Move(targetTile));
        }
    }

    public void Reset()
    {
        IsMoving = false;
        IsActive = false;
        StopAllCoroutines();
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

            if (targetTile.AvailableToMoveThroughSide(movingSide) && nextTile.AvailableToMoveThroughSide(enterSide) && !targetTile.Exit)
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
        Debug.Log("click");
        if (LevelSaveManager.LevelNumber == 2) return;
        
        switchButton.ActivateCharacter();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            CurrentTile = col.GetComponent<Tile>();
            if (!CurrentTile.IsGrass && !CurrentTile.WaterLily && !CurrentTile.isFeeshOnTile && !IsMoving || CurrentTile.IsEdge)
            {
                WinLoseManager.Lose.SetActive(true);
            }
            if (IsActive)
            {
                HighlightTiles();
            }
        }
        
        if (col.CompareTag("Spike"))
        {
            WinLoseManager.Lose.SetActive(true);
        }
        
        if (col.CompareTag("Lever"))
        {
            col.GetComponent<Lever>().Switch();
        }
    }

    private void OnDestroy()
    {
        Count--;
        CharacterManager.Jumper = null;
    }
}
