using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RestartSingleplayerButton : MonoBehaviour
{
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private GameObject window;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Restart);
    }

    private void Restart()
    {
        TileManager.SetTiles();
        characterManager.ResetCharacters();
        window.SetActive(false);
        StepCounter.Count = 0;
    }
}
