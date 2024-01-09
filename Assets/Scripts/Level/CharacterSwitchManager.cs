using UnityEngine;

public class CharacterSwitchManager : MonoBehaviour
{
    [SerializeField] private CharacterSwitchButton sonicButton;
    [SerializeField] private CharacterSwitchButton jumperButton;
    [SerializeField] private CharacterSwitchButton feeshButton;

    private void Update()
    {
        sonicButton.gameObject.SetActive(CharacterManager.Sonic is not null && CharacterManager.Sonic.gameObject.activeSelf);
        jumperButton.gameObject.SetActive(CharacterManager.Jumper is not null && CharacterManager.Jumper.gameObject.activeSelf);
        feeshButton.gameObject.SetActive(CharacterManager.Feesh is not null && CharacterManager.Feesh.gameObject.activeSelf);
    }
}
