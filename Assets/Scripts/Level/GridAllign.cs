using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAllign : MonoBehaviour
{
	private void Awake()
	{
		var cellPos = ((Vector3Int)LevelSaveManager.LoadedLevel.FieldSize + Vector3.one * 2) / 2;
		Debug.Log($"central cell: {cellPos}");
		transform.position = -cellPos;
	}
}
