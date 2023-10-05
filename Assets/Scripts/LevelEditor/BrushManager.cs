using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    public static Action<Tile> CurrentBrush;

    private void Update()
    {
        if (CurrentBrush is null) return;
        
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
            CurrentBrush.Invoke(targetTile);
        }
    }
}
