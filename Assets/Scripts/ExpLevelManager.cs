using UnityEngine;

public static class ExpLevelManager
{
    public static int MaxExp => GetLevelUpExp(PlayerLevel + 1);

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
            Debug.Log($"exp {PlayerData.Exp}");
            if (PlayerData.Exp >= MaxExp)
            {
                PlayerData.Exp %= MaxExp;
                PlayerLevel++;
                Debug.Log($"lvl up exp:{PlayerData.Exp} lvl:{PlayerLevel}");
            }
            
            RealtimeDatabase.PushUserData();
        }
    }

    private static int GetLevelUpExp(int level)
    {
        if (level == 1)
        {
            return 80;
        }

        return GetLevelUpExp(level - 1) + (level - level % 2) * 10;
    }
}
