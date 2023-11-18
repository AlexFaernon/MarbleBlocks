using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorLevelStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> editorUI;
    [SerializeField] private List<GameObject> levelUI;
    [SerializeField] private Physics2DRaycaster characterRaycaster;
    [SerializeField] private Physics2DRaycaster editorRaycaster;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelSaveManager levelSaveManager;
    private bool _isTesting;
    
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
