using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RealtimeDatabase : MonoBehaviour
{
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    public static bool LevelLoaded;
    public static bool UserLoaded;
    public static bool LeaderboardLoaded;

    private static DataSnapshot _leaderboardSnapshot;
    
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
    
    public static IEnumerator ExportRandomLevel()
    {
        LevelLoaded = false;
        LevelClass level = null;
        var opponent = GetRandomOpponent();
        Debug.Log(opponent);
        var loadLevel = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(opponent).Child("Map").GetValueAsync();
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
        var keys = _leaderboardSnapshot.Children.Select(child => child.Key).ToList();
        if (keys.Count <= 0) return null;
        var randomIndex = Random.Range(0, keys.Count);
        return keys[randomIndex];
    }
    
    public static IEnumerator ExportUserData()
    {
        UserLoaded = false;
        PlayerClass player = null;
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
                var dataJson = snapshot.GetRawJsonValue();
                player = JsonConvert.DeserializeObject<PlayerClass>(dataJson);
                Debug.Log("Player loaded");
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
        Debug.Log("PLayer and rank loaded");
        PlayerData.PlayerClass = player;
        UserLoaded = true;
    }

    public static void PushMap(LevelClass level, bool isCompleted)
    {
        var json = JsonConvert.SerializeObject(level);
        Debug.Log("Почта " + AuthManager.User.Email);
        var path = isCompleted ? "Map" : "EditorMap";
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child(path).SetRawJsonValueAsync(json);
    }

    public static void PushUserData(PlayerClass playerClass)
    {
        var userJson = JsonConvert.SerializeObject(playerClass);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).SetRawJsonValueAsync(userJson);
    }

    public static void PushRank()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(AuthManager.User.DisplayName).SetRawJsonValueAsync("{\"Item1\":0,\"Item2\":" + PlayerData.Rank + "}");
    }
}

