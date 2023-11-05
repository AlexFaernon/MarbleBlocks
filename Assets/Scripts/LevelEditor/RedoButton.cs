using UnityEngine;
using UnityEngine.UI;

public class RedoButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        _button.interactable = Drawer.Redo.Count > 0;
    }

    private void OnClick()
    {
        if (Drawer.Redo.Count == 0) return;
        Drawer.Redo.Pop()();
    }
}
