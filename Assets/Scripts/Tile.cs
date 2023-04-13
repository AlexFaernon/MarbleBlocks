using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isGrass;
    [SerializeField] private WallClass northWall;
    [SerializeField] private WallClass southWall;
    [SerializeField] private WallClass westWall;
    [SerializeField] private WallClass eastWall;
    public Vector2Int gridPosition;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
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
}
