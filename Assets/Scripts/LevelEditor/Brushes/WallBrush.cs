using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WallBrush : Brush
{
	[SerializeField] private TMP_Text limitLabel;
	[SerializeField] private bool isDoor;
	[SerializeField] private bool isOpened;
	private void Awake()
	{
		Button = GetComponent<Button>();
		Button.onClick.AddListener(OnClick);
		Image = GetComponent<Image>();
		UnselectedSprite = Image.sprite;
		if (isDoor && PlayerData.SingleLevelCompleted < LevelObjectsLimits.GateLevel)
		{
			gameObject.SetActive(false);
		}
	}
	
	public override bool Draw(Tile tile)
	{
		if (!Button.interactable) return false;
		
		PlaceWall(tile, Side, null);
		return true;
	}

	private void Update()
	{
		Image.sprite = Drawer.CurrentBrush == this ? selectedSprite : UnselectedSprite;
		if (!isDoor) return;

		Button.interactable = Wall.DoorCount < LevelObjectsLimits.Gate;
		limitLabel.text = (LevelObjectsLimits.Gate - Wall.DoorCount).ToString();
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