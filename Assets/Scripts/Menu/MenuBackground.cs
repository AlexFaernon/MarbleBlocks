using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private Sprite area2Sprite;
    [SerializeField] private Sprite area3Sprite;
    [SerializeField] private float area2SwitchThreshold;
    [SerializeField] private float area3SwitchThreshold;
    private Sprite _area1Sprite;
    private Image _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<Image>();
        _area1Sprite = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (scroll.horizontalNormalizedPosition < area2SwitchThreshold)
        {
            _spriteRenderer.sprite = _area1Sprite;
        }
        else if (scroll.horizontalNormalizedPosition >= area2SwitchThreshold && scroll.horizontalNormalizedPosition < area3SwitchThreshold)
        {
            _spriteRenderer.sprite = area2Sprite;
        }
        else
        {
            _spriteRenderer.sprite = area3Sprite;
        }
    }
}
