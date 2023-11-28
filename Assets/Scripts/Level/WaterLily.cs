using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLily : MonoBehaviour
{
    public static int Count;

    private void OnEnable()
    {
        Count++;
    }

    private void OnDisable()
    {
        Count--;
    }
}
