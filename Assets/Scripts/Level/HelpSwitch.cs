using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpSwitch : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject jumper;
    [SerializeField] private GameObject sonic;
    [SerializeField] private GameObject feesh;
    [SerializeField] private GameObject arrow;
    public int MaxHelpLevels => _helpGroups.Count;
    private readonly List<Action<bool>> _helpGroups = new();
    private Toggle _toggle;
    private GameObject _jumper;
    private GameObject _jumperArrow;
    private GameObject _sonic;
    private GameObject _sonicArrow;
    private GameObject _feesh;
    public int HelpLevel { get; private set; }

    private void Awake()
    {
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer && LevelSaveManager.LevelNumber < 4)
        {
            gameObject.SetActive(false);
            return;
        }
        

        CreateHelp();
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer && PlayerPrefs.HasKey($"Help{LevelSaveManager.LevelNumber}"))
        {
            HelpLevel = PlayerPrefs.GetInt($"Help{LevelSaveManager.LevelNumber}");
        }
        _toggle = GetComponent<Toggle>();
        if (HelpLevel == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void BuyHelp()
    {
        if (PlayerData.Name.ToUpper() != "TRW")
        {
            CoinsManager.Coins--;
        }
        
        HelpLevel++;
        if (GameMode.CurrentGameMode == GameModeType.SinglePlayer)
        {
            PlayerPrefs.SetInt($"Help{LevelSaveManager.LevelNumber}", HelpLevel);
        }
    }
    
    private void Update()
    {
        _toggle.image.enabled = !_toggle.isOn;

        for (var i = 0; i < HelpLevel; i++)
        {
            _helpGroups[i](_toggle.isOn);
        }
    }

    private void CreateHelp()
    {
        var help = LevelSaveManager.LoadedLevel.HelpClass;
        
        if (help.JumperPos != Vector2Int.zero)
        {
            var jumperPos = grid.GetCellCenterWorld((Vector3Int)help.JumperPos);
            _jumperArrow = CreateArrow(help.JumperPos, help.JumperDirection);
            _jumper = Instantiate(jumper, jumperPos, Quaternion.identity);
            _jumperArrow.SetActive(false);
            _jumper.SetActive(false);
            _helpGroups.Add(isActive =>
                {
                    _jumperArrow.SetActive(isActive);
                    _jumper.SetActive(isActive);
                });
        }

        if (help.SonicPos != Vector2Int.zero)
        {
            var sonicPos = grid.GetCellCenterWorld((Vector3Int)help.SonicPos);
            _sonicArrow = CreateArrow(help.SonicPos, help.SonicDirection);
            _sonic = Instantiate(sonic, sonicPos, Quaternion.identity);
            _sonicArrow.SetActive(false);
            _sonic.SetActive(false);
            _helpGroups.Add(isActive =>
                {
                    _sonicArrow.SetActive(isActive);
                    _sonic.SetActive(isActive);
                });
        }
		
        if (help.FeeshPos != Vector2Int.zero)
        {
            var feeshPos = grid.GetCellCenterWorld((Vector3Int)help.FeeshPos);
            _feesh = Instantiate(feesh, feeshPos, Quaternion.identity);
            _feesh.SetActive(false);
            _helpGroups.Add(isActive =>
                {
                    _feesh.SetActive(isActive);
                });
        }
    }
    
    private GameObject CreateArrow(Vector2Int pos, Side direction)
    {
        var offset = direction switch
        {
            Side.North => Vector2Int.up,
            Side.South => Vector2Int.down,
            Side.West => Vector2Int.left,
            Side.East => Vector2Int.right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        var angle = direction switch
        {
            Side.North => 0,
            Side.South => 180,
            Side.West => 90,
            Side.East => -90,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
		
        var worldPos = grid.GetCellCenterWorld((Vector3Int)(pos + offset));
        return Instantiate(arrow, worldPos, Quaternion.Euler(0, 0, angle));
    }
}
