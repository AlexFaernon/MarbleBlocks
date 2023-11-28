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
        Destroy(CharacterManager.Sonic.gameObject);
        Destroy(CharacterManager.Jumper.gameObject);
        Destroy(CharacterManager.Feesh.gameObject);
    }
}
