using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brushes : MonoBehaviour
{
    private static bool _isGrass;
    
    public void PlaceGrass()
    {
        _isGrass = true;
        BrushManager.CurrentBrush = ChangeGround;
    }

    public void PlaceWater()
    {
        _isGrass = false;
        BrushManager.CurrentBrush = ChangeGround;
    }

    private static void ChangeGround(Tile tile)
    {
        tile.IsGrass = _isGrass;
    }
}
