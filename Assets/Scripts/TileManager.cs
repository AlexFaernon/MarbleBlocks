using System;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    public Vector2Int fieldSize;
    [SerializeField] private bool loadLevel;
    private Grid _grid;
    private static TileClass[,] _loadedTiles;
    private static Tile[,] _tiles;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        if (loadLevel)
        {
            fieldSize = SaveManager.LoadedLevel.FieldSize;
        }
        _tiles = new Tile[fieldSize.x + 2, fieldSize.y + 2];
        if (loadLevel)
        {
            _loadedTiles = SaveManager.LoadedLevel.Tiles;
        }

        BuildLevel();
    }

    private void BuildLevel()
    {
        for (var x = 0; x < fieldSize.x + 2; x++)
        {
            for (var y = 0; y < fieldSize.y + 2; y++)
            {
                var intPos = new Vector2Int(x, y);
                var pos = _grid.GetCellCenterWorld((Vector3Int)intPos);
                _tiles[x, y] = Instantiate(tilePrefab, pos, Quaternion.identity).GetComponent<Tile>();
                _tiles[x, y].gridPosition = intPos;
                if (x == 0 || x == fieldSize.x + 1 || y == 0 || y == fieldSize.y + 1)
                {
                    _tiles[x, y].isEdge = true;
                }
                else if (loadLevel)
                {
                    _tiles[x, y].TileClass = _loadedTiles[x - 1, y - 1];
                }
                else
                {
                    _tiles[x, y].isGrass = true;
                }
            }
        }
    }

    public TileClass[,] GetSave()
    {
        var tiles = new TileClass[fieldSize.x, fieldSize.y];

        foreach (var tile in _tiles)
        {
            if (tile.isEdge) continue;
            
            tiles[tile.gridPosition.x - 1, tile.gridPosition.y - 1] = tile.GetSave();
        }

        return tiles;
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

    public static void HighlightTiles(HashSet<Tile> highlightedTiles)
    {
        foreach (var tile in _tiles)
        {
            tile.IsHighlighted = highlightedTiles.Contains(tile);
        }
    }
}
