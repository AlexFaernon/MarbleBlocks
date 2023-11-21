using System;
using UnityEngine;
using UnityEngine.UI;

public class GroundBrush : Brush
{
	[SerializeField] private bool isGrass;

	private void Awake()
	{
		Button = GetComponent<Button>();
		Button.onClick.AddListener(OnClick);
		Image = GetComponent<Image>();
		UnselectedSprite = Image.sprite;
	}

	private void Update()
	{
		Image.sprite = Drawer.CurrentBrush == this ? selectedSprite : UnselectedSprite;
	}

	public override bool Draw(Tile tile)
	{
		if (!Button.interactable) return false;
		
		if (Check(tile, isGrass))
		{
			ChangeGround(tile, isGrass);
			return true;
		}

		return false;
	}
	
	private static void ChangeGround(Tile tile, bool isGrass)
	{
		tile.IsGrass = isGrass;
		void Undo() => RevertGround(tile, isGrass);
		Drawer.Undo.Push(Undo);
	}

	private static void RevertGround(Tile tile, bool isGrass)
	{
		tile.IsGrass = !isGrass;
		void Redo() => ChangeGround(tile, isGrass);
		Drawer.Redo.Push(Redo);
	}
}