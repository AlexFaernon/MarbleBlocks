using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        _button.interactable = Drawer.Undo.Count > 0;
    }
    
    private void OnClick()
    {
        if (Drawer.Undo.Count == 0) return;
        Drawer.Undo.Pop()();
    }
}
