using UnityEngine;
using UnityEngine.UI;

public class ClearLevel : MonoBehaviour
{
    [SerializeField] private GameObject sizeSelector;
    [SerializeField] private CameraClamp cameraClamp;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Drawer.Undo.Clear();
        Drawer.Redo.Clear();
        if (CharacterManager.Sonic != null) Destroy(CharacterManager.Sonic.gameObject);
        if (CharacterManager.Jumper != null) Destroy(CharacterManager.Jumper.gameObject);
        if (CharacterManager.Feesh != null) Destroy(CharacterManager.Feesh.gameObject);
        cameraClamp.ResetCameraPos();
        sizeSelector.SetActive(true);
    }
}
