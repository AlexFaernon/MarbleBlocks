using UnityEngine;
using UnityEngine.UI;

public class LocationSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject location1;
    [SerializeField] private GameObject location2;

    [SerializeField] private Button up;
    [SerializeField] private Button down;
    private void Start()
    {
        location1.SetActive(true);
        location2.SetActive(false);
        
        up.onClick.AddListener(SwitchLocation);
        down.onClick.AddListener(SwitchLocation);
    }

    private void SwitchLocation()
    {
        location1.SetActive(!location1.activeSelf);
        location2.SetActive(!location2.activeSelf);
    }
}
