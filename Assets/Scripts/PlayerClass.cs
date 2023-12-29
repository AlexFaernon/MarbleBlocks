using System;
using System.Collections.Generic;

public class PlayerClass
{
	public int Level = 0;
	public int Exp = 0;
	public int Coins = 0;
	public int SingleLevelCompleted;
	public string LastLogin;
	public Dictionary<string, Tuple<int, bool>> AchievementsAndQuest = null;
}
