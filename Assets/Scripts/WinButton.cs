using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }
}
