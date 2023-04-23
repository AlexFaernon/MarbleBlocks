using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Feesh") || col.CompareTag("Sonic") || col.CompareTag("Jumper"))
        {
            Debug.Log("Exit!");
        }
    }
}
