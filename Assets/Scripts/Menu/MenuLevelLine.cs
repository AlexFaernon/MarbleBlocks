using UnityEngine;
using UnityEngine.UI;

public class MenuLevelLine : MonoBehaviour
{
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite completed;
    [SerializeField] private MenuLevelCircle nextLevelCircle;

    private void Start()
    {
        if (nextLevelCircle.isCompleted)
        {
            GetComponent<Image>().sprite = completed;
        }
        else if (nextLevelCircle.isOpened)
        {
            GetComponent<Image>().sprite = opened;
        }
        else
        {
            GetComponent<Image>().sprite = closed;
        }
    }
}
