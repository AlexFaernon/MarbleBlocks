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
        if (PlayerData.SingleLevelCompleted >= nextLevelCircle.levelNumber)
        {
            GetComponent<Image>().sprite = completed;
        }
        else if (nextLevelCircle.levelNumber - 1 == PlayerData.SingleLevelCompleted)
        {
            GetComponent<Image>().sprite = opened;
        }
        else
        {
            GetComponent<Image>().sprite = closed;
        }
    }
}
