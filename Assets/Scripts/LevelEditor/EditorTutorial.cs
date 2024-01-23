using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorTutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorial;
    [SerializeField] private Button levelSizeButton;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("OpenedEditor"))
        {
            levelSizeButton.onClick.AddListener(() => StartCoroutine(ShowTutorial()));
        }
    }

    private IEnumerator ShowTutorial()
    {
        yield return new WaitUntil(() => TileManager.EditorFieldSize != Vector2Int.zero);
        
        tutorial.GetChild(0).gameObject.SetActive(true);
        
        var current = tutorial.GetChild(1).gameObject;
        current.SetActive(true);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.touchCount == 0);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || GetTouchDown());
        current.SetActive(false);
        
        current = tutorial.GetChild(2).gameObject;
        current.SetActive(true);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.touchCount == 0);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || GetTouchDown());
        current.SetActive(false);
        
        current = tutorial.GetChild(3).gameObject;
        current.SetActive(true);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.touchCount == 0);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || GetTouchDown());
        current.SetActive(false);
        
        current = tutorial.GetChild(4).gameObject;
        current.SetActive(true);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => Input.touchCount == 0);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || GetTouchDown());
        current.SetActive(false);
        
        tutorial.GetChild(0).gameObject.SetActive(false);
        PlayerPrefs.SetInt("OpenedEditor", 1);
    }
    
    private bool GetTouchDown()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}
