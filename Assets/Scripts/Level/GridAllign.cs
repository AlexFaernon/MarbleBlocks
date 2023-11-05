using UnityEngine;

public class GridAllign : MonoBehaviour
{
	[SerializeField] private TileManager tileManager;
	private void Awake()
	{
		if (tileManager.loadLevel)
		{
			var cellPos = ((Vector3Int)LevelSaveManager.LoadedLevel.FieldSize + Vector3.one * 2) / 2;
			transform.position = -cellPos;
		}
		else
		{
			var cellPos = ((Vector3Int)tileManager.fieldSize + Vector3.one * 2) / 2;
			transform.position = -cellPos;
		}
	}
}
