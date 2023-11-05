using UnityEngine;
using UnityEngine.UI;

public class LevelSreenshot : MonoBehaviour
{
	private Image _image;
	private void Awake()
	{
		_image = GetComponent<Image>();
	}
	private void OnEnable()
	{
		_image.sprite = Resources.Load<Sprite>($"LevelScreenshots\\{LevelSaveManager.LevelNumber}");
	}
}
