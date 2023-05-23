using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitLevelInfo : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => transform.parent.gameObject.SetActive(false));
    }
}
