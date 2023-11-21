using System;
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
	public abstract bool Draw(Tile tile);

	protected void OnClick()
	{
		Drawer.CurrentBrush = this;
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
		
		return !tile.Spike && !tile.isJumperOnTile && !tile.isSonicOnTile;
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
