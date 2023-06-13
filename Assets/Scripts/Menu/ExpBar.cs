using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text tmpText;

    private void Update()
    {
        tmpText.text = $"Level {ExpLevelManager.PlayerLevel}";
        fill.fillAmount = (float)ExpLevelManager.Exp / ExpLevelManager.MaxExp;
    }
}
