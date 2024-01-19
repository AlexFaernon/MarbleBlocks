using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
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
    public Vector2Int GridPosition => CurrentTile.gridPosition;
    //[SerializeField] private Animator animator;
    public static int Count;
    
    private SkeletonAnimationMulti skeletonAnimation;
    private MeshRenderer mesh;
    private bool facingRight = true;

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
        
        skeletonAnimation = GetComponent<SkeletonAnimationMulti>();
    }

    public void StartMoving(Side side)
    {
        Assert.IsTrue(IsActive);
        
        if (IsMoving || CurrentTile.isJumperOnTile) return;

        if (CanMoveForward(CurrentTile, side))
        {
            StepCounter.Count++;
            if (GameMode.CurrentGameMode == GameModeType.LevelEditor)
            {
                WriteHelpInEditor.PushSonicMove(GridPosition, side);
            }
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
        {
            FlipCharacter(true);
            skeletonAnimation.SetAnimation("sova side jump", true);
        }     
        if (movingVector == Vector2.up)
        {
        }     
        if (movingVector == Vector2.down)
        {
            skeletonAnimation.SetAnimation("sova fas jump", true);
            skeletonAnimation.CurrentSkeletonAnimation.AnimationState.TimeScale = 2;
        }     
        if (movingVector == Vector2.left)
        {
            FlipCharacter(false);
            skeletonAnimation.SetAnimation("sova side jump", true);
        }   
        
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
                IsMoving = false;
                skeletonAnimation.ClearAnimation();
                skeletonAnimation.SetAnimation("sova afk", true);
                mesh = skeletonAnimation.transform.GetChild(0).GetComponent<MeshRenderer>(); //todo в awake эта херня еще не появилась, позже нужно перенести отсюда
                mesh.sortingOrder = 6;
            }
        }
        if (!CurrentTile.IsGrass && !CurrentTile.WaterLily && !CurrentTile.isFeeshOnTile && !IsMoving || CurrentTile.IsEdge)
        {
            WinLoseManager.Lose.SetActive(true);
        }
    }

    private void FlipCharacter(bool jumpRight)
    {
        if ((jumpRight && !facingRight) || (!jumpRight && facingRight))
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
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

        if (nextTile is null)
        {
            return false;
        }
        
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
            if (CurrentTile.IsEdge)
            {
                WinLoseManager.Lose.SetActive(true);
            }
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
