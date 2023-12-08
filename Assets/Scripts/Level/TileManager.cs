using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform field7X7;
    [SerializeField] private Transform field9X9;
    [SerializeField] private Transform field11X11;
    public static Vector2Int EditorFieldSize; //todo fix camera on editor
    public static bool LoadLevel;
    private Grid _grid;
    private static Tile[,] _tiles;
    private static Tile[,] _tiles7X7;
    private static Tile[,] _tiles9X9;
    private static Tile[,] _tiles11X11;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        switch (GameMode.CurrentGameMode)
        {
            case GameModeType.SinglePlayer:
                Debug.Log("Loading Single");
                _tiles = CreateTiles(_grid.transform, LevelSaveManager.LoadedLevel.FieldSize);
                SetTiles();
                break;
            case GameModeType.MultiPlayer:
                _tiles7X7 = CreateTiles(field7X7, new Vector2Int(7,7));
                _tiles9X9 = CreateTiles(field9X9, new Vector2Int(9,9));
                _tiles11X11 = CreateTiles(field11X11, new Vector2Int(11,11));
                break;
            case GameModeType.LevelEditor: //todo пофиксить отображение нижних тайлов в редакторе
                LoadEditorLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Tile[,] CreateTiles(Transform tileParent, Vector2Int fieldSize)
    {
        var tiles = new Tile[fieldSize.x + 2, fieldSize.y + 2];
        for (var x = 0; x < fieldSize.x + 2; x++)
        {
            for (var y = 0; y < fieldSize.y + 2; y++)
            {
                var intPos = new Vector2Int(x, y);
                var pos = _grid.GetCellCenterWorld((Vector3Int)intPos);
                tiles[x, y] = Instantiate(tilePrefab, pos, Quaternion.identity, tileParent).GetComponent<Tile>();
                tiles[x, y].gridPosition = intPos;
                if (x == 0 || x == fieldSize.x + 1 || y == 0 || y == fieldSize.y + 1)
                {
                    tiles[x, y].IsEdge = true;
                }
            }
        }
        return tiles;
    }

    public void LoadEditorLevel()
    {
        if (LevelSaveManager.LoadedLevel is null)
        {
            Debug.Log("level not found");
            _tiles = CreateTiles(_grid.transform, EditorFieldSize);
            LevelSaveManager.LoadedLevel = new LevelClass { FieldSize = EditorFieldSize };
        }
        else
        {
            Debug.Log("level found");
            _tiles = CreateTiles(_grid.transform, LevelSaveManager.LoadedLevel.FieldSize);
            EditorFieldSize = LevelSaveManager.LoadedLevel.FieldSize;
            SetTiles();
        }
    }
    
    public static void SetTiles()
    {
        for (var x = 1; x < LevelSaveManager.LoadedLevel.FieldSize.x + 1; x++)
        {
            for (var y = 1; y < LevelSaveManager.LoadedLevel.FieldSize.y + 1; y++)
            {
                _tiles[x, y].TileClass = LevelSaveManager.LoadedLevel.Tiles[x - 1, y - 1];
            }
        }
    }

    public void SwitchTileSetForMultiplayer()
    {
        field7X7.gameObject.SetActive(false);
        field9X9.gameObject.SetActive(false);
        field11X11.gameObject.SetActive(false);

        switch (LevelSaveManager.LoadedLevel.FieldSize)
        {
            case { x: 7, y: 7 }:
                field7X7.gameObject.SetActive(true);
                _tiles = _tiles7X7;
                SetTiles();
                break;
            case { x: 9, y: 9 }:
                field9X9.gameObject.SetActive(true);
                _tiles = _tiles9X9;
                SetTiles();
                break;
            case { x: 11, y: 11 }:
                field11X11.gameObject.SetActive(true);
                _tiles = _tiles11X11;
                SetTiles();
                break;
            default:
                throw new ArgumentException("incorrect field size");
        }
    }

    public TileClass[,] GetSave()
    {
        var tiles = new TileClass[EditorFieldSize.x, EditorFieldSize.y];

        foreach (var tile in _tiles)
        {
            if (tile.IsEdge) continue;
            
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

    public void RemakeLevelInEditor()
    {
        foreach (var tile in _tiles)
        {
            Destroy(tile.gameObject);
        }
        _tiles = CreateTiles(_grid.transform, EditorFieldSize);
    }

    public static void HighlightTiles(HashSet<Tile> highlightedTiles)
    {
        foreach (var tile in _tiles)
        {
            tile.IsHighlighted = highlightedTiles.Contains(tile);
        }
    }
}
