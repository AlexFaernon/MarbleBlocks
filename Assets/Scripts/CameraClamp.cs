using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
	[SerializeField] private Grid grid;
	private Vector3 CameraCenter
	{
		get
		{
			var fieldSize = GameMode.CurrentGameMode == GameModeType.LevelEditor ? TileManager.EditorFieldSize : LevelSaveManager.LoadedLevel.FieldSize;
			var result = grid.GetCellCenterWorld((Vector3Int)(fieldSize / 2 + Vector2Int.one));
			result.z = -10;
			return result;
		}
	}

	private void Start()
	{
		if (GameMode.CurrentGameMode == GameModeType.SinglePlayer)
		{
			ResetCameraPos();
		}
	}

	private void Update()
	{
		var clamp = new Vector2(3, 3);
		var pos = transform.position;
		var cameraCenter = CameraCenter;
		pos.x = Math.Clamp(pos.x, -clamp.x + cameraCenter.x, clamp.x + cameraCenter.x);
		pos.y = Math.Clamp(pos.y, -clamp.y + cameraCenter.y, clamp.y + cameraCenter.y);
		transform.position = pos;
	}

	public void ResetCameraPos()
	{
		transform.position = CameraCenter;
	}
}
