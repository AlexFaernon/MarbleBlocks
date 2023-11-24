using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorLevelStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> editorUI;
    [SerializeField] private List<GameObject> levelUI;
    [SerializeField] private Physics2DRaycaster characterRaycaster;
    [SerializeField] private Physics2DRaycaster editorRaycaster;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelSaveManager levelSaveManager;
    private bool _isTesting;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        _button.interactable = Exit.Count > 0 && (Sonic.Count == 1 || Jumper.Count == 1 || Feesh.Count == 1);
    }

    public void Switch()
    {
        _isTesting = !_isTesting;
        TileManager.HighlightTiles(new HashSet<Tile>());
        if (_isTesting)
        {
            levelSaveManager.SaveLevel();
        }
        else
        {
            TileManager.ResetLevel();
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
        
        if (CharacterManager.Sonic)
        {
            CharacterManager.Sonic.enabled = _isTesting;
        }
        
        if (CharacterManager.Jumper)
        {
            CharacterManager.Jumper.enabled = _isTesting;
        }
        
        if (CharacterManager.Feesh)
        {
            CharacterManager.Feesh.enabled = _isTesting;
        }
    }
}
