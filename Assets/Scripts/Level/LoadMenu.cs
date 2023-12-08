using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }
}
