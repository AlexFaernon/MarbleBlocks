using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WallBrush : Brush
{
	[SerializeField] private bool isDoor;
	[SerializeField] private bool isOpened;
	private void Awake()
	{
		Button = GetComponent<Button>();
		Button.onClick.AddListener(OnClick);
	}
	
	public override void Draw(Tile tile)
	{
		if (!Button.interactable) return;
		
		PlaceWall(tile, Side, null);
	}
	
	private void PlaceWall(Tile tile, Side side, WallClass wallClass)
	{
		var wall = tile.GetWall(side);
		var oldWall = wall.GetSave();
		wallClass ??= new WallClass
		{
			IsActive = true,
			IsDoor = isDoor,
			IsOpened = isOpened,
			Color = Color
		};
		wall.WallClass = wallClass;
        
		Drawer.Undo.Push(() => RevertWall(tile, side, oldWall));
	}

	private void RevertWall(Tile tile, Side side, WallClass oldWall)
	{
		var wall = tile.GetWall(side);
		var newWall = wall.GetSave();
		wall.WallClass = oldWall;
        
		Drawer.Redo.Push(() => PlaceWall(tile, side, newWall));
	}

}