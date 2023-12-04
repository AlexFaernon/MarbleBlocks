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
        _label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (PlayerData.Name is not null)
        {
            _label.text = PlayerData.Name;
        }
    }
}
