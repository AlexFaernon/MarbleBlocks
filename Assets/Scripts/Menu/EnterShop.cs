using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterShop : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OpenShop);
    }

    private void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
