using UnityEngine;

public class GridAllign : MonoBehaviour
{
	[SerializeField] private TileManager tileManager;
	private void Awake()
	{
		var cellPos = ((Vector3Int)tileManager.fieldSize + Vector3.one * 2) / 2;
		transform.position = -cellPos;
	}
}
