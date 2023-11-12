using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterBrush : Brush
{
	[SerializeField] private Grid grid;
	[SerializeField] private GameObject selectedCharacter;

	private void Awake()
	{
		Button = GetComponent<Button>();
		Button.onClick.AddListener(OnClick);
	}

	public override void Draw(Tile tile)
	{
		if (!Button.interactable) return;
		
		PlaceCharacter(tile, selectedCharacter);
	}

	private void Update()
	{
		Button.interactable = selectedCharacter.tag switch
		{
			"Sonic" => Sonic.Count < 1,
			"Jumper" => Jumper.Count < 1,
			"Feesh" => Feesh.Count < 1,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private void PlaceCharacter(Tile tile, GameObject character)
	{
		var pos = grid.GetCellCenterWorld((Vector3Int)tile.gridPosition);
		var spawnedCharacter = Instantiate(character, pos, quaternion.identity);
		if (spawnedCharacter.TryGetComponent<Sonic>(out var sonic))
		{
			sonic.enabled = false;
		}
		else if (spawnedCharacter.TryGetComponent<Jumper>(out var jumper))
		{
			jumper.enabled = false;
		}
		else if (spawnedCharacter.TryGetComponent<Feesh>(out var feesh))
		{
			feesh.enabled = false;
		}
        
		Drawer.Undo.Push(() => RemoveCharacter(tile, character, spawnedCharacter));
	}

	private void RemoveCharacter(Tile tile, GameObject character, GameObject characterToRemove)
	{
		Destroy(characterToRemove);
		Drawer.Redo.Push(() => PlaceCharacter(tile, character));
	}
}