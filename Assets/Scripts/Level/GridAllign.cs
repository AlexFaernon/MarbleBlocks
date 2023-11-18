using UnityEngine;

public class GridAllign : MonoBehaviour
{
	private void Awake()
	{
		var cellPos = ((Vector3Int)TileManager.fieldSize + Vector3.one * 2) / 2;
		transform.position = -cellPos;
	}
}
