using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isEdge;
    public bool isGrass;
    [SerializeField] private WallClass northWall;
    [SerializeField] private WallClass southWall;
    [SerializeField] private WallClass westWall;
    [SerializeField] private WallClass eastWall;
    [SerializeField] private GameObject whirlpool;
    [HideInInspector] public Vector2Int gridPosition;
    private SpriteRenderer _spriteRenderer;
    [HideInInspector] public bool isFeeshOnTile;
    [HideInInspector] public bool isJumperOnTile;
    [HideInInspector] public bool isSonicOnTile;
    public bool IsWhirlpoolOnTile => whirlpool.activeSelf;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isEdge)
        {
            _spriteRenderer.color = Color.clear;
            return;
        }
        
        _spriteRenderer.color = isGrass ? Color.green : Color.blue;
    }

    public bool AvailableToMoveThroughSide(Side side)
    {
        return side switch
        {
            Side.North => northWall.AvailableToMove(),
            Side.South => southWall.AvailableToMove(),
            Side.West => westWall.AvailableToMove(),
            Side.East => eastWall.AvailableToMove(),
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Feesh"))
        {
            isFeeshOnTile = true;
        }

        if (col.CompareTag("Jumper"))
        {
            isJumperOnTile = true;
        }
        
        if (col.CompareTag("Sonic"))
        {
            isSonicOnTile = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Feesh"))
        {
            isFeeshOnTile = false;
        }

        if (col.CompareTag("Jumper"))
        {
            isJumperOnTile = false;
        }

        if (col.CompareTag("Sonic"))
        {
            isSonicOnTile = false;
        }
    }
}
