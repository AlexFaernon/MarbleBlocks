using System.Collections.Generic;

public class TileClass
{
    public bool IsGrass;
    public OnTileObject OnTileObject;
    public LeverClass LeverClass;
    public TeleportClass TeleportClass;
    public Dictionary<Side, WallClass> Walls;
}
