using UnityEngine;
using UnityEngine.UI;

public class SizeSelector : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Sprite selected;
    private Image _image;
    private Sprite _unselected;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => TileManager.EditorFieldSize = size);
        _image = GetComponent<Image>();
        _unselected = _image.sprite;
    }

    private void Update()
    {
        _image.sprite = size == TileManager.EditorFieldSize ? selected : _unselected;
    }
}
