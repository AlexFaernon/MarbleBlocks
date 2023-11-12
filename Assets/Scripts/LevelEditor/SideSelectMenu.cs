using System;
using UnityEngine;
using UnityEngine.UI;

public class SideSelectMenu : MonoBehaviour
{
	[SerializeField] private Image sideIcon;
	[SerializeField] private GameObject sideButtons;
	[SerializeField] private Sprite north;
	[SerializeField] private Sprite south;
	[SerializeField] private Sprite east;
	[SerializeField] private Sprite west;
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	private void Update()
	{
		sideIcon.sprite = Brush.Side switch
		{
			Side.North => north,
			Side.South => south,
			Side.West => west,
			Side.East => east,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private void OnClick()
	{
		sideButtons.SetActive(!sideButtons.activeSelf);
	}
}
