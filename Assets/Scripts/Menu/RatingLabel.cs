using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RatingLabel : MonoBehaviour
{
    private TMP_Text _label;

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _label.text = PlayerData.Rank.ToString();
    }
}
