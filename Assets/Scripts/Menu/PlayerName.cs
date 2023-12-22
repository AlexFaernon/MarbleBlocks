using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    private Button _button;
    private TMP_Text _label;

    private void Awake()
    {
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
