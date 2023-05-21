using System.Collections.Generic;

public class TileClass
{
    public bool IsGrass;
    public OnTileObject OnTileObject;
    public LeverClass LeverClass;
    public Dictionary<Side, WallClass> Walls;
}
