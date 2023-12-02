using UnityEngine;
using UnityEngine.UI;

public class SizeSelector : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Image selected;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => TileManager.EditorFieldSize = size);
    }

    private void Update()
    {
        selected.gameObject.SetActive(size == TileManager.EditorFieldSize);
    }
}
