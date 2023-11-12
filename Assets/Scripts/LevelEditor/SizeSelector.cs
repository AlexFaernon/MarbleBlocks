using UnityEngine;
using UnityEngine.UI;

public class SizeSelector : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Image selected;
    [SerializeField] private TileManager tileManager;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => tileManager.fieldSize = size);
    }

    private void Update()
    {
        selected.gameObject.SetActive(size == tileManager.fieldSize);
    }
}
