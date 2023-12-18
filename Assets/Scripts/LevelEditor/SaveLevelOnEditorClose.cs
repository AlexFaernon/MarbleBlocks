using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevelOnEditorClose : MonoBehaviour
{
	[SerializeField] private LevelSaveManager levelSaveManager;
	private void OnDestroy()
	{
		levelSaveManager.SaveLevel();
	}
}
