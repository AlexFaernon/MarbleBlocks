using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardProgressBar : MonoBehaviour
{
    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text rankLabel;
    private const float FillTime = 1.5f;
    private bool _progressBarFilled;

    private IEnumerator FillProgressbar(float oldValue, float newValue, float max)
    {
        _progressBarFilled = false;
        var value = oldValue;
        var difference = newValue - oldValue;
        
        while (value < newValue)
        {
            value += difference / FillTime * Time.deltaTime;
            fill.fillAmount = value / max;
            if (rankLabel is not null)
            {
                rankLabel.text = $"{(int)value}/{max}";
            }
            yield return new WaitForEndOfFrame();
        }
        _progressBarFilled = true;
    }

    public IEnumerator GainExp(int expGain)
    {
        var oldValue = ExpLevelManager.Exp;
        var oldMax = ExpLevelManager.MaxExp;
        var oldLevel = ExpLevelManager.PlayerLevel;
        ExpLevelManager.Exp += expGain;
        if (oldMax < ExpLevelManager.MaxExp)
        {
            playerLevel.text = oldLevel.ToString();
            StartCoroutine(FillProgressbar(oldValue, oldMax, oldMax));
            yield return new WaitUntil(() => _progressBarFilled);

            oldValue = 0;
        }
        playerLevel.text = ExpLevelManager.PlayerLevel.ToString();
        StartCoroutine(FillProgressbar(oldValue, ExpLevelManager.Exp, ExpLevelManager.MaxExp));
    }

    public IEnumerator GainRank(int oldRank, int newRank)
    {
        var value = oldRank;
        var oldMax = PlayerData.GetNextRankThreshold(oldRank);
        var newMax = PlayerData.GetNextRankThreshold(newRank);
        if (oldMax < newMax)
        {
            StartCoroutine(FillProgressbar(value, oldMax, oldMax));
            yield return new WaitUntil(() => _progressBarFilled);

            value = 0;
        }
        StartCoroutine(FillProgressbar(value, newRank, newMax));
    }
}
