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
    [SerializeField] private CameraClamp cameraClamp;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => StartCoroutine(FindLevel(false)));
        if (GameMode.CurrentGameMode == GameModeType.MultiPlayer)
        {
            StartCoroutine(FindLevel(true));
        }
    }

    private IEnumerator FindLevel(bool loadPrevious)
    {
        _button.interactable = false;
        StartCoroutine(RealtimeDatabase.ExportRandomLevel(loadPrevious));
        yield return new WaitUntil(() => RealtimeDatabase.LevelLoaded);
        tileManager.SwitchTileSetForMultiplayer();
        characterManager.ResetCharacters();
        _button.interactable = true;
        opponentInfo.UpdateInfo();
        cameraClamp.ResetCameraPos();
    }
}