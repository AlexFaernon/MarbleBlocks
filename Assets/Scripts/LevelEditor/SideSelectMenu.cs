using System;
using UnityEngine;
using UnityEngine.UI;

public class SideSelectMenu : MonoBehaviour
{
	[SerializeField] private Image sideIcon;
	[SerializeField] private Sprite selectedSprite;
	private Sprite _unselectedSprite;
	private Image _image;
	[SerializeField] private GameObject sideButtons;
	[SerializeField] private Sprite north;
	[SerializeField] private Sprite south;
	[SerializeField] private Sprite east;
	[SerializeField] private Sprite west;
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
		_image = GetComponent<Image>();
		_unselectedSprite = _image.sprite;
	}

	private void Update()
	{
		_image.sprite = sideButtons.activeSelf ? selectedSprite : _unselectedSprite;
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
