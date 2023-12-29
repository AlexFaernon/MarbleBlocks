using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Random = UnityEngine.Random;

public class RealtimeDatabase : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    public static bool LevelLoaded;
    public static bool UserLoaded;
    public static bool LeaderboardLoaded;
    public static bool HistoryLoaded;

    private static DataSnapshot _leaderboardSnapshot;
    public static string Opponent { get; private set; }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        {
            if (task.Exception != null)
            {
                Debug.Log($"Error {task.Exception}");
                return;
            }
            OnFirebaseInitialized.Invoke();
        });
    }

    public static void PushEnergy(EnergyTimestamp energyTimestamp)
    {
        var json = JsonConvert.SerializeObject(energyTimestamp);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("Energy").SetRawJsonValueAsync(json);
    }
    
    public static IEnumerator ExportRandomLevel()
    {
        LevelLoaded = false;
        LevelClass level = null;
        Opponent = GetRandomOpponent();
        Debug.Log(Opponent);
        var loadLevel = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(Opponent).Child("Map").GetValueAsync();
        loadLevel.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    var levelJson = snapshot.GetRawJsonValue();
                    level = JsonConvert.DeserializeObject<LevelClass>(levelJson);
                    Debug.Log("Level loaded");
                }
            });
        yield return new WaitUntil(() => level is not null && loadLevel.IsCompleted);

        LevelSaveManager.LoadedLevel = level;
        LevelLoaded = true;
    }
    
    public static IEnumerator ExportEditorLevel()
    {
        LevelLoaded = false;
        LevelClass level = null;
        var taskCompeted = false;
        var loadLevel = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(PlayerData.Name).Child("EditorMap").GetValueAsync();
        loadLevel.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        var levelJson = snapshot.GetRawJsonValue();
                        level = JsonConvert.DeserializeObject<LevelClass>(levelJson);
                        Debug.Log("Level loaded");
                    }
                }
                taskCompeted = true;
            });
        yield return new WaitUntil(() => taskCompeted);

        LevelSaveManager.LoadedLevel = level;
        LevelLoaded = true;
    }

     public static IEnumerator ExportLeaderboard()
     {
         LeaderboardLoaded = false;
         Dictionary<string, Tuple<int, int>> leaderboard = null;
         var leaderboardLoaded = false;
         var loadLeaderboard = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").GetValueAsync();
         loadLeaderboard.ContinueWithOnMainThread(task =>
         {
             if (task.IsFaulted)
             {
                 Debug.Log($"Error {task.Exception}");
             }
             else if (task.IsCompleted)
             {
                 _leaderboardSnapshot = task.Result;
                 if (_leaderboardSnapshot.Exists)
                 {
                     var leaderboardJson = _leaderboardSnapshot.GetRawJsonValue();
                     leaderboard = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, int>>>(leaderboardJson);
                     Debug.Log("Leaderboard loaded");
                 }
             }
             leaderboardLoaded = true;
         });
         yield return new WaitUntil(() => leaderboardLoaded);
         LeaderboardManager.LeaderboardData = leaderboard;
         LeaderboardLoaded = true;
     }
     
    private static string GetRandomOpponent()
    {
        var keys = _leaderboardSnapshot.Children
            .Select(child => child.Key)
            .Where(name => name != AuthManager.User.DisplayName && name != Opponent)
            .ToList();
        if (keys.Count <= 0) return null;
        var randomIndex = Random.Range(0, keys.Count);
        return keys[randomIndex];
    }
    
    public static IEnumerator ExportUserData()
    {
        UserLoaded = false;
        var playerLoaded = false;
        var rankLoaded = false;
        var loadData = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).GetValueAsync();
        var loadRank = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(AuthManager.User.DisplayName).Child("Item2").GetValueAsync();
        loadData.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log($"Error {task.Exception}");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                try
                {
                    var dataJson = snapshot.GetRawJsonValue();
                    PlayerData.PlayerClass = JsonConvert.DeserializeObject<PlayerClass>(dataJson);
                    Debug.Log("Player loaded");
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                var energySnapshot = snapshot.Child("Energy");
                EnergyManager.EnergyTimestamp = JsonConvert.DeserializeObject<EnergyTimestamp>(energySnapshot.GetRawJsonValue());
                Debug.Log("Energy loaded");

                var shopSnapshot = snapshot.Child("Shop");
                LevelObjectsLimits.ObjectLimitClass = JsonConvert.DeserializeObject<ObjectLimitClass>(shopSnapshot.GetRawJsonValue());
                Debug.Log("Shop loaded");
            }
            playerLoaded = true;
        });
        loadRank.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        PlayerData.Rank = int.Parse(snapshot.Value.ToString());
                        Debug.Log("Rank loaded");
                    }
                    else
                    {
                        Debug.Log("Rank not found");
                    }
                }
                rankLoaded = true;
            });
        yield return new WaitUntil(() => playerLoaded && rankLoaded);
        Debug.Log("PLayer, energy and rank loaded");
        UserLoaded = true;
    }

    public static void PushMap(LevelClass level, bool isCompleted)
    {
        var json = JsonConvert.SerializeObject(level);
        Debug.Log("Почта " + AuthManager.User.Email);
        var path = isCompleted ? "Map" : "EditorMap";
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child(path).SetRawJsonValueAsync(json);
    }

    public static void PushUserData()
    {
        // var userJson = JsonConvert.SerializeObject(PlayerData.PlayerClass);
        // var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(userJson);
        // dict["AchievementsAndQuest"] = ((JObject)dict["AchievementsAndQuest"]).ToObject<Dictionary<string, Tuple<int, bool>>>();
        var dict = new Dictionary<string, object>();
        var playerClass = PlayerData.PlayerClass;
        foreach (var field in typeof(PlayerClass).GetFields())
        {
            if (field.Name != "AchievementsAndQuest")
            {
                dict[field.Name] = field.GetValue(playerClass);
            }
        }
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).UpdateChildrenAsync(dict);
        var jsonValue = JsonConvert.SerializeObject(playerClass.AchievementsAndQuest);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("AchievementsAndQuest").SetRawJsonValueAsync(jsonValue);
    }

    public static void PushInitialUserData()
    {
        var userJson = JsonConvert.SerializeObject(PlayerData.PlayerClass);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).SetRawJsonValueAsync(userJson);
        var energyJson = JsonConvert.SerializeObject(EnergyManager.EnergyTimestamp);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("Energy").SetRawJsonValueAsync(energyJson);
        var shopJson = JsonConvert.SerializeObject(new ObjectLimitClass());
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("Shop").SetRawJsonValueAsync(shopJson);
    }

    public static void PushShop()
    {
        var json = JsonConvert.SerializeObject(LevelObjectsLimits.ObjectLimitClass);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("Shop").SetRawJsonValueAsync(json);
    }

    public static IEnumerator IncreaseLevelCount()
    {
        var loadRank = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(AuthManager.User.DisplayName).Child("Item1").GetValueAsync();
        var countLoaded = false;
        var levels = 0;
        
        loadRank.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        levels = int.Parse(snapshot.Value.ToString());
                        Debug.Log("Rank loaded");
                    }
                    else
                    {
                        Debug.Log("Rank not found");
                    }
                }
                countLoaded = true;
            });
        yield return new WaitUntil(() => countLoaded);

        levels++;
        FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(AuthManager.User.DisplayName).Child("Item1").SetRawJsonValueAsync(levels.ToString());
    }
    
    public static IEnumerator PushRank(string playerName, int rankDelta)
    {
        var loadRank = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(playerName).Child("Item2").GetValueAsync();
        var rankLoaded = false;
        var rank = 0;
        
        loadRank.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        rank = int.Parse(snapshot.Value.ToString());
                        Debug.Log($"{playerName}'s Rank loaded");
                    }
                    else
                    {
                        Debug.Log($"{playerName}'s Rank not found");
                    }
                }
                rankLoaded = true;
            });
        yield return new WaitUntil(() => rankLoaded);

        rank = Math.Clamp(rank + rankDelta, 0, int.MaxValue);
        if (playerName == AuthManager.User.DisplayName)
        {
            PlayerData.Rank = rank;
        }
        FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(playerName).Child("Item2").SetRawJsonValueAsync(rank.ToString());
        Debug.Log("Rank pushed");
    }

    public static void PushToHistory(string opponentName, int stepCount, bool isWin)
    {
        var pair = JsonConvert.SerializeObject(new Tuple<int, bool>(stepCount, isWin));
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(opponentName).Child("History").Child(AuthManager.User.DisplayName).SetRawJsonValueAsync(pair);
    }

    public static IEnumerator ExportHistory()
    {
        HistoryLoaded = false;
        var historyTask = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("History").GetValueAsync();
        var historyLoaded = false;
        Dictionary<string, Tuple<int, bool>> history = null;
        
        historyTask.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    var historySnapshot = task.Result;
                    if (historySnapshot.Exists)
                    {
                        var historyJson = historySnapshot.GetRawJsonValue();
                        history = JsonConvert.DeserializeObject<Dictionary<string, Tuple<int, bool>>>(historyJson);
                        Debug.Log("History loaded");
                    }
                }
                historyLoaded = true;
            });
        yield return new WaitUntil(() => historyLoaded);

        HistoryManager.History = history;
        HistoryLoaded = true;
    }
}

