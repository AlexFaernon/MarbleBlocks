using UnityEngine;

public class GridAllign : MonoBehaviour
{
	private void Update()
	{
		var cellPos = ((Vector3Int)LevelSaveManager.LoadedLevel.FieldSize + Vector3.one * 2) / 2;
		transform.position = -cellPos;
	}
}
