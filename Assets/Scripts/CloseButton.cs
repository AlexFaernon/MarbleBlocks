using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => button.transform.parent.gameObject.SetActive(false));
    }
}
