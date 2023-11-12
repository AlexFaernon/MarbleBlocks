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
	}

	public override void Draw(Tile tile)
	{
		if (!Button.interactable) return;
		
		ChangeGround(tile, isGrass);
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