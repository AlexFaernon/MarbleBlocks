using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextRandomLevel : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private MultiplayerOpponentInfo opponentInfo;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => StartCoroutine(FindLevel()));
        StartCoroutine(FindLevel());
    }

    private IEnumerator FindLevel()
    {
        _button.interactable = false;
        StartCoroutine(RealtimeDatabase.ExportRandomLevel());
        yield return new WaitUntil(() => RealtimeDatabase.LevelLoaded);
        tileManager.SwitchTileSetForMultiplayer();
        characterManager.ResetCharacters();
        _button.interactable = true;
        opponentInfo.UpdateInfo();
    }
}
