using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private TMP_Text _label;
    public static int Coins
    {
        get => PlayerData.Coins;
        set
        {
            PlayerData.Coins = value;
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
        if (Coins != -1) return;
        
        if (PlayerPrefs.HasKey("Coins"))
        {
            Coins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            Coins = 0;
        }
    }

    private void Update()
    {
        _label.text = Coins.ToString();
    }
}
