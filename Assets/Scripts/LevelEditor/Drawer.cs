using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drawer : MonoBehaviour
{
    public static Action<Tile> CurrentBrush;
    public static readonly Stack<Action> Undo = new();
    public static readonly Stack<Action> Redo = new();
    
    private void Update()
    {
        if (CurrentBrush is null) return;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        Vector2 ray;
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touch = Input.GetTouch(0);
            ray = Camera.main.ScreenToWorldPoint(touch.position);
        }
        else
        {
            return;
        }

        var layerObject = LayerMask.GetMask("Ground", "UI");
        
        var hit = Physics2D.Raycast(ray, Vector2.zero, Mathf.Infinity, layerObject);
        
        if (hit.collider is not null)
        {
            var targetTile = hit.collider.gameObject.GetComponent<Tile>();
            Redo.Clear();
            CurrentBrush(targetTile);
        }
    }
}
