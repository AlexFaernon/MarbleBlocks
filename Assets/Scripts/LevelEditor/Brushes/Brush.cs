using UnityEngine;
using UnityEngine.UI;

public abstract class Brush : MonoBehaviour
{
	public static DoorLeverColor Color;
	public static Side Side;
	protected Button Button;
	public abstract void Draw(Tile tile);

	protected void OnClick()
	{
		Drawer.CurrentBrush = Draw;
	}
}
