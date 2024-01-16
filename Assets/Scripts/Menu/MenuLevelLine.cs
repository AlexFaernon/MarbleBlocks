using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevelLine : MonoBehaviour
{
    [SerializeField] private Material solid;
    [SerializeField] private Material dotted;
    [SerializeField] private MenuLevelCircle nextLevelCircle;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (PlayerData.SingleLevelCompleted >= nextLevelCircle.levelNumber)
        {
            _lineRenderer.material = solid;
        }
        else if (nextLevelCircle.levelNumber - 1 == PlayerData.SingleLevelCompleted)
        {
            _lineRenderer.material = solid;
        }
        else
        {
            _lineRenderer.material = dotted;
        }
    }
}
