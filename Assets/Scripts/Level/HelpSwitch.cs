using Newtonsoft.Json;
using System;
using System.Collections;
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
    private HelpClass _help;
    private Toggle _toggle;
    private GameObject _jumper;
    private GameObject _jumperArrow;
    private GameObject _sonic;
    private GameObject _sonicArrow;
    private GameObject _feesh;
    public int HelpLevel { get; private set; }

    private void Awake()
    {
        if (LevelSaveManager.LevelNumber < 4)
        {
            gameObject.SetActive(false);
            return;
        }
        
        var helpJson = Resources.Load<TextAsset>($"Help\\{LevelSaveManager.LevelNumber}").text;
        _help = JsonConvert.DeserializeObject<HelpClass>(helpJson);
        CreateHelp();
        if (PlayerPrefs.HasKey($"Help{LevelSaveManager.LevelNumber}"))
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
        if (NameManager.PlayerName.ToUpper() != "TRW")
        {
            CoinsManager.Coins--;
        }
        
        HelpLevel++;
        PlayerPrefs.SetInt($"Help{LevelSaveManager.LevelNumber}", HelpLevel);
    }
    
    private void Update()
    {
        _toggle.image.enabled = !_toggle.isOn;
        
        if (!_toggle.isOn)
        {
            _jumper.SetActive(false);
            _jumperArrow.SetActive(false);
            _sonic.SetActive(false);
            _sonicArrow.SetActive(false);
            _feesh.SetActive(false);
            return;
        }
        
        if (HelpLevel > 0)
        {
            _jumper.SetActive(true);
            _jumperArrow.SetActive(true);
        }

        if (HelpLevel > 1)
        {
            _sonic.SetActive(true);
            _sonicArrow.SetActive(true);
        }
		
        if (HelpLevel > 2)
        {
            _feesh.SetActive(true);
        }
    }

    private void CreateHelp()
    {
        var jumperPos = grid.GetCellCenterWorld((Vector3Int)_help.JumperPos);
        _jumperArrow = CreateArrow(_help.JumperPos, _help.JumperDirection);
        _jumper = Instantiate(jumper, jumperPos, Quaternion.identity);
        _jumperArrow.SetActive(false);
        _jumper.SetActive(false);

        var sonicPos = grid.GetCellCenterWorld((Vector3Int)_help.SonicPos);
        _sonicArrow = CreateArrow(_help.SonicPos, _help.SonicDirection);
        _sonic = Instantiate(sonic, sonicPos, Quaternion.identity);
        _sonicArrow.SetActive(false);
        _sonic.SetActive(false);
		
        var feeshPos = grid.GetCellCenterWorld((Vector3Int)_help.FeeshPos);
        _feesh = Instantiate(feesh, feeshPos, Quaternion.identity);
        _feesh.SetActive(false);
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
