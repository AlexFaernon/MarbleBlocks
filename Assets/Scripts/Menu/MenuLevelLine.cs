using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevelLine : MonoBehaviour
{
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite completed;
    [SerializeField] private MenuLevelCircle nextLevelCircle;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (PlayerData.SingleLevelCompleted >= nextLevelCircle.levelNumber)
        {
            image.sprite = completed;
        }
        else if (nextLevelCircle.levelNumber - 1 == PlayerData.SingleLevelCompleted)
        {
            image.sprite = opened;
        }
        else
        {
            image.sprite = closed;
        }
    }
}
