using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject sonic;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject feesh;
    [SerializeField] private Grid grid;
    public static Sonic Sonic;
    public static Jumper Jumper;
    public static Feesh Feesh;

    private void Awake()
    {
        switch (GameMode.CurrentGameMode)
        {
            case GameModeType.SinglePlayer:
                if (LevelSaveManager.LoadedLevel.SonicPosition != Vector2Int.zero)
                {
                    var sonicPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.SonicPosition);
                    Instantiate(sonic, sonicPos, Quaternion.identity, grid.transform);
                }
        
                if (LevelSaveManager.LoadedLevel.JumperPosition != Vector2Int.zero)
                {
                    var jumperPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.JumperPosition);
                    Instantiate(jumper, jumperPos, Quaternion.identity, grid.transform);
                }
        
                if (LevelSaveManager.LoadedLevel.FeeshPosition != Vector2Int.zero)
                {
                    var feeshPos = grid.GetCellCenterWorld((Vector3Int)LevelSaveManager.LoadedLevel.FeeshPosition);
                    Instantiate(feesh, feeshPos, Quaternion.identity, grid.transform);
                }
                break;
            case GameModeType.MultiPlayer:
                var pos = grid.GetCellCenterWorld((Vector3Int)Vector2Int.one);
                Instantiate(sonic, pos, Quaternion.identity, grid.transform).SetActive(false);
                Instantiate(jumper, pos, Quaternion.identity, grid.transform).SetActive(false);
                Instantiate(feesh, pos, Quaternion.identity, grid.transform).SetActive(false);
                break;
            case GameModeType.LevelEditor:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ResetCharacters()
    {
        if (Sonic)
        {
            Sonic.Reset();
            var sonicPosition = LevelSaveManager.LoadedLevel.SonicPosition;
            Sonic.transform.position = grid.GetCellCenterWorld((Vector3Int)sonicPosition);
            Sonic.gameObject.SetActive(sonicPosition != Vector2Int.zero);
        }
        
        if (Jumper)
        {
            Jumper.Reset();
            var jumperPosition = LevelSaveManager.LoadedLevel.JumperPosition;
            Jumper.transform.position = grid.GetCellCenterWorld((Vector3Int)jumperPosition);
            Jumper.gameObject.SetActive(jumperPosition != Vector2Int.zero);
        }

        if (Feesh)
        {
            Feesh.Reset();
            var feeshPosition = LevelSaveManager.LoadedLevel.FeeshPosition;
            Feesh.transform.position = grid.GetCellCenterWorld((Vector3Int)feeshPosition);
            Feesh.gameObject.SetActive(feeshPosition != Vector2Int.zero);
        }
    }
}
