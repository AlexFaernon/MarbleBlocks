using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StepCounter : MonoBehaviour
{
    public static int Count;
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        Count = 0;
    }

    private void Update()
    {
        label.text = Count.ToString();
    }
}
