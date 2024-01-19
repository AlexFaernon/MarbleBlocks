using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Brush : MonoBehaviour
{
	[SerializeField] protected Sprite selectedSprite;
	public static DoorLeverColor Color;
	public static Side Side;
	protected Button Button;
	protected Image Image;
	protected Sprite UnselectedSprite;

	protected readonly Dictionary<DoorLeverColor, bool> AllowedColors = new()
	{
		{ DoorLeverColor.Red, true },
		{ DoorLeverColor.Grey, true },
		{ DoorLeverColor.Green, true },
		{ DoorLeverColor.Purple, true },
		{ DoorLeverColor.Blue, true },
	};
	public abstract bool Draw(Tile tile);

	protected void OnClick()
	{
		Drawer.CurrentBrush = this;
		ColorSelect.AllowedColors = AllowedColors;
	}
	
	public bool Check(Tile tile, OnTileObject onTileObject)
	{
		switch (onTileObject)
		{
			case OnTileObject.None:
				throw new ArgumentException("Checking none object");
			case OnTileObject.Spike:
				return tile.IsGrass && !tile.AnyCharacterOnTile;
			case OnTileObject.Whirlpool:
				return !tile.IsGrass && !tile.AnyCharacterOnTile;
			case OnTileObject.Lever:
				return !tile.AnyCharacterOnTile;
			case OnTileObject.Exit:
				return !tile.AnyCharacterOnTile;
			case OnTileObject.WaterLily:
				return !tile.AnyCharacterOnTile && !tile.IsGrass;
			case OnTileObject.Teleport:
				return !tile.AnyCharacterOnTile && tile.IsGrass;
			default:
				throw new ArgumentOutOfRangeException(nameof(onTileObject), onTileObject, null);
		}
	}

	public bool Check(Tile tile, bool isGrass)
	{
		if (isGrass)
		{
			return !tile.isFeeshOnTile && !tile.Whirlpool;
		}
		
		return !tile.Spike && tile.Teleport is null && !tile.isJumperOnTile && !tile.isSonicOnTile;
	}

	public bool Check(Tile tile, string character)
	{
		return character switch
		{
			"Sonic" or "Jumper" => tile.IsGrass && !tile.Spike && !tile.Exit && tile.Lever is null,
			"Feesh" => !tile.IsGrass && !tile.Whirlpool && !tile.Exit && tile.Lever is null,
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}
