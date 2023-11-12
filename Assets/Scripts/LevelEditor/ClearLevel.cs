using UnityEngine;
using UnityEngine.UI;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Drawer.Undo.Clear();
        Drawer.Redo.Clear();
        tileManager.ClearAllTiles();
        Destroy(GameObject.FindWithTag("Sonic"));
        Destroy(GameObject.FindWithTag("Jumper"));
        Destroy(GameObject.FindWithTag("Feesh"));
    }
}
