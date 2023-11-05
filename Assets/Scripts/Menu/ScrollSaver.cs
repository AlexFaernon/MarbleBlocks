using UnityEngine;
using UnityEngine.UI;

public class ScrollSaver : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("Scroll"))
        {
            GetComponent<ScrollRect>().verticalNormalizedPosition = PlayerPrefs.GetFloat("Scroll");
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("Scroll", GetComponent<ScrollRect>().verticalNormalizedPosition);
    }
}
