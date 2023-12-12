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
            
            RealtimeDatabase.PushUserData();
        }
    }

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _label.text = Coins.ToString();
    }
}
