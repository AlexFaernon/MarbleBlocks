using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button confirm;

    public static string PlayerName = "";

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            PlayerName = PlayerPrefs.GetString("PlayerName");
        }
        
        if (PlayerName != "")
        {
            gameObject.SetActive(false);
        }
        confirm.onClick.AddListener(Confirm);
    }

    private void Update()
    {
        confirm.interactable = inputField.text != "";
    }

    private void Confirm()
    {
        PlayerName = inputField.text;
        PlayerPrefs.SetString("PlayerName", PlayerName);
        gameObject.SetActive(false);
    }
}
