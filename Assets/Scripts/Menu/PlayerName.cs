using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private GameObject nameWindow;
    private Button _button;
    private TMP_Text _label;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => nameWindow.SetActive(true));
        _label = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (NameManager.PlayerName is not null)
        {
            _label.text = NameManager.PlayerName;
        }
    }
}
