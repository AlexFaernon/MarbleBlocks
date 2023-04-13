using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private Vector2Int fieldSize;
    private Grid _grid;
    private static Tile[,] _tiles;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _tiles = new Tile[fieldSize.x, fieldSize.y];
        for (var x = 0; x < fieldSize.x; x++)
        {
            for (var y = 0; y < fieldSize.y; y++)
            {
                var intPos = new Vector2Int(x, y);
                var pos = _grid.GetCellCenterWorld((Vector3Int)intPos);
                _tiles[x, y] = Instantiate(tile, pos, Quaternion.identity).GetComponent<Tile>();
                _tiles[x, y].gridPosition = intPos;
                _tiles[x, y].isGrass = true;
            }
        }
    }

    public static Tile GetTile(Tile currentTile, Side nearTitleSide)
    {
        var pos = currentTile.gridPosition;
        pos += nearTitleSide switch
        {
            Side.North => Vector2Int.up,
            Side.South => Vector2Int.down,
            Side.West => Vector2Int.left,
            Side.East => Vector2Int.right,
            _ => throw new ArgumentOutOfRangeException(nameof(nearTitleSide), nearTitleSide, null)
        };

        if (0 <= pos.x && pos.x < _tiles.GetLength(0) && 0 <= pos.y && pos.y < _tiles.GetLength(1))
        {
            return _tiles[pos.x, pos.y];
        }

        return null;
    }
}
