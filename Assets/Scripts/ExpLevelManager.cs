public static class ExpLevelManager
{
    public const int MaxExp = 1000;
    public static int PlayerLevel
    {
        get => PlayerData.Level;
        private set
        {
            PlayerData.Level = value;
            EnergyManager.CurrentEnergy++;
        }
    }
    
    public static int Exp
    {
        get => PlayerData.Exp;
        set
        {
            PlayerData.Exp = value;
            if (PlayerData.Exp >= MaxExp)
            {
                PlayerLevel++;
                PlayerData.Exp %= MaxExp;
            }
            
            RealtimeDatabase.PushUserData();
        }
    }
}
