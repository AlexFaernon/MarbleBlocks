using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
	[SerializeField] private Grid grid;
	[SerializeField] private GameObject jumper;
	[SerializeField] private GameObject sonic;
	[SerializeField] private GameObject feesh;
	[SerializeField] private GameObject arrow;
	private HelpClass _help;
	private GameObject _jumper;
	private GameObject _jumperArrow;
	private GameObject _sonic;
	private GameObject _sonicArrow;
	private GameObject _feesh;
	private void Awake()
	{
		var helpJson = Resources.Load<TextAsset>($"Help\\{LevelSaveManager.LevelNumber.ToString()}").text;
		_help = JsonConvert.DeserializeObject<HelpClass>(helpJson);
		GetComponent<Button>().onClick.AddListener(ShowHelp);
		CreateHelp();
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
	
	private void ShowHelp()
	{
		transform.parent.gameObject.SetActive(false);
		
		if (!_jumper.activeSelf)
		{
			_jumper.SetActive(true);
			_jumperArrow.SetActive(true);
			return;
		}

		if (!_sonic.activeSelf)
		{
			_sonic.SetActive(true);
			_sonicArrow.SetActive(true);
			return;
		}
		
		_feesh.SetActive(true);
	}
}
