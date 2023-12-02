using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            {
                GameMode.CurrentGameMode = GameModeType.LevelEditor;
                SceneManager.LoadScene("LevelEditor");
            });
    }
}
