public static class LevelObjectsLimits
{
    public static ObjectLimitClass ObjectLimitClass
    {
        get => new()
        {
            Lever = Lever,
            Spike = Spike,
            Whirlpool = Whirlpool,
            WaterLily = WaterLily,
            Teleport = Teleport
        };
        set
        {
            Lever = value.Lever;
            Lever = value.Lever;
            Spike = value.Spike;
            Whirlpool = value.Whirlpool;
            WaterLily = value.WaterLily;
            Teleport = value.Teleport;
        }
    }
    
    public static int Lever = 1;
    public static int Exit = 10;
    public static int Spike = 5;
    public static int Whirlpool = 5;
    public static int WaterLily = 5;
    public static int Teleport = 5;
}
