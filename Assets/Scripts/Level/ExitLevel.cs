using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitLevel : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }
}
