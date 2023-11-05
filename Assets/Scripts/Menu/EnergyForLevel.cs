using TMPro;
using UnityEngine;

public class EnergyForLevel : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        text.text = LevelSaveManager.LevelNumber <= 3 ? "-0" : "-1";
    }
}
