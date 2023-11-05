using UnityEngine;
using UnityEngine.UI;

public class SwitchColorSideMenu : MonoBehaviour
{
    [SerializeField] private bool isColorShown;
    [SerializeField] private GameObject color;
    [SerializeField] private bool isSideShown;
    [SerializeField] private GameObject side;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        color.SetActive(isColorShown);
        side.SetActive(isSideShown);
    }
}
