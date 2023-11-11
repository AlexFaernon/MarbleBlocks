using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorLevelStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> editorUI;
    [SerializeField] private List<GameObject> levelUI;
    [SerializeField] private Physics2DRaycaster characterRaycaster;
    [SerializeField] private Physics2DRaycaster editorRaycaster;
    private bool _isTesting;
    private Sonic _sonic;
    private Jumper _jumper;
    private Feesh _feesh;

    public void Switch()
    {
        _isTesting = !_isTesting;
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

        if (!_isTesting)
        {
            if (_sonic)
            {
                _sonic.enabled = false; 
            }
            _sonic = null;
            if (_jumper)
            {
                _jumper.enabled = false;
            }
            _jumper = null;
            if (_feesh)
            {
                _feesh.enabled = false;
            }
            _feesh = null;
            return;
        }
        
        var sonicObj = GameObject.FindWithTag("Sonic");
        if (sonicObj)
        {
            _sonic =  sonicObj.GetComponent<Sonic>();
            _sonic.enabled = true;
        }
        var jumperObj = GameObject.FindWithTag("Jumper");
        if (jumperObj)
        {
            _jumper = jumperObj.GetComponent<Jumper>();
            _jumper.enabled = true;
        }
        
        var feeshObj = GameObject.FindWithTag("Feesh");
        if (feeshObj)
        {
            _feesh = feeshObj.GetComponent<Feesh>();
            _feesh.enabled = true;
        }
    }
}
