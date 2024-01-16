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
            Teleport = Teleport,
            Gate = Gate,
            Purple = Purple,
            Green = Green,
            Blue = Blue
        };
        set
        {
            Lever = value.Lever;
            Lever = value.Lever;
            Spike = value.Spike;
            Whirlpool = value.Whirlpool;
            WaterLily = value.WaterLily;
            Teleport = value.Teleport;
            Gate = value.Gate;
            Purple = value.Purple;
            Green = value.Green;
            Blue = value.Blue;
        }
    }
    
    public const int LeverLevel = 8;
    public const int GateLevel = 8;
    public const int TeleportLevel = 15;
    public const int WaterlilyLevel = 9;
    public const int BlueLevel = 0;
    public const int PurpleLevel = 0;
    public const int GreenLevel = 0;
    
    public static int Lever = 1;
    public static int Spike = 2;
    public static int Whirlpool = 2;
    public static int WaterLily = 2;
    public static int Teleport = 2;
    public static int Gate = 2;
    public static bool Purple;
    public static bool Green;
    public static bool Blue;
}
