using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Training : MonoBehaviour
{
    [SerializeField] private GameObject blocker;
    [SerializeField] private Transform feeshTraining;
    [SerializeField] private Transform jumperTraining;

    private void Start()
    {
        if (LevelSaveManager.LevelNumber == 1)
        {
            StartCoroutine(FeeshTraining());
        }

        if (LevelSaveManager.LevelNumber == 3)
        {
            StartCoroutine(JumperTraining());
        }
    }

    private IEnumerator FeeshTraining()
    {
        feeshTraining.gameObject.SetActive(true);
        
        var current = feeshTraining.GetChild(0).gameObject;
        current.SetActive(true);
        var feesh = GameObject.FindWithTag("Feesh").GetComponent<Feesh>();
        yield return new WaitUntil(() => feesh.IsActive);
        current.SetActive(false);
        
        current = feeshTraining.GetChild(1).gameObject;
        var level = GameObject.FindWithTag("Lever").GetComponent<Lever>();
        current.SetActive(true);
        yield return new WaitUntil(() => level.IsActive);
        current.SetActive(false);
        
        current = feeshTraining.GetChild(2).gameObject;
        blocker.SetActive(true);
        current.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.touchCount > 0);
        current.SetActive(false);

        yield return new WaitForEndOfFrame();
        
        current = feeshTraining.GetChild(3).gameObject;
        current.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.touchCount > 0);
        current.SetActive(false);
        blocker.SetActive(false);

        current = feeshTraining.GetChild(4).gameObject;
        current.SetActive(true);
    }

    private IEnumerator JumperTraining()
    {
        jumperTraining.gameObject.SetActive(true);
        var jumper = GameObject.FindWithTag("Jumper").GetComponent<Jumper>();
        
        var current = jumperTraining.GetChild(0).gameObject;
        yield return new WaitForEndOfFrame();
        current.SetActive(true);
        yield return new WaitUntil(() => jumper.GetGridPosition == new Vector2Int(5,5));
        current.SetActive(false);
        
        current = jumperTraining.GetChild(1).gameObject;
        blocker.SetActive(true);
        current.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.touchCount > 0);
        current.SetActive(false);
        blocker.SetActive(false);
        
        current = jumperTraining.GetChild(2).gameObject;
        current.SetActive(true);
        yield return new WaitUntil(() => jumper.GetGridPosition == new Vector2Int(7, 5));
        current.SetActive(false);
        
        yield return new WaitUntil(() => jumper.GetGridPosition == new Vector2Int(5, 1));

        current = jumperTraining.GetChild(3).gameObject;
        current.SetActive(true);
    }
}
