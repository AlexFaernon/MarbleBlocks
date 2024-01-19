using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EditorLevelStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> editorUI;
    [SerializeField] private List<GameObject> levelUI;
    [SerializeField] private Physics2DRaycaster characterRaycaster;
    [SerializeField] private Physics2DRaycaster editorRaycaster;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelSaveManager levelSaveManager;
    [SerializeField] private CameraClamp cameraClamp;
    [SerializeField] private ZoomPinch zoomPinch;
    [SerializeField] private Sprite stopIcon;
    private bool _isTesting;
    private Button _button;
    private Image _icon;
    private Sprite _startIcon;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _icon = transform.GetChild(0).GetComponent<Image>();
        _startIcon = _icon.sprite;
    }

    private void Update()
    {
        _button.interactable = Exit.Count > 0 && (Sonic.Count == 1 || Jumper.Count == 1 || Feesh.Count == 1);
        _icon.sprite = _isTesting ? stopIcon : _startIcon;
    }

    public void Switch()
    {
        zoomPinch.ResetCameraZoom();
        cameraClamp.ResetCameraPos();
        
        _isTesting = !_isTesting;
        TileManager.HighlightTiles(new HashSet<Tile>());
        if (_isTesting)
        {
            StepCounter.Count = 0;
            levelSaveManager.SaveLevel();
            WriteHelpInEditor.ResetHelp();
        }
        else
        {
            TileManager.SetTiles();
            characterManager.ResetCharacters();
        }
        
        Drawer.CurrentBrush = null;
        characterRaycaster.enabled = _isTesting;
        editorRaycaster.enabled = !_isTesting;
        
        foreach (var go in editorUI)
        {
            go.SetActive(!_isTesting);
        }

        foreach (var go in levelUI)
        {
            go.SetActive(_isTesting);
        }
    }
}
