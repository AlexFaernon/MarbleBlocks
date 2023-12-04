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

    private static DataSnapshot leaderboard; //todo затычка, снести когда класс напишем
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

     public static IEnumerator ExportLeaderboard()
     {
         LeaderboardLoaded = false;
         var loadLeaderboard = FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").GetValueAsync();
         loadLeaderboard.ContinueWithOnMainThread(task =>
         {
             if (task.IsFaulted)
             {
                 Debug.Log($"Error {task.Exception}");
             }
             else if (task.IsCompleted)
             {
                 DataSnapshot snapshot = task.Result;
                 var leaderboardJson = snapshot.GetRawJsonValue();
                 leaderboard = snapshot;
                 Debug.Log("Leaderboard loaded");
             }
         });
         yield return new WaitUntil(() => leaderboard is not null && loadLeaderboard.IsCompleted);
         LeaderboardLoaded = true;
     }
     
    private static string GetRandomOpponent()
    {
        var keys = leaderboard.Children.Select(child => child.Key).ToList();
        if (keys.Count <= 0) return null;
        var randomIndex = Random.Range(0, keys.Count);
        return keys[randomIndex];
    }
    
    public static IEnumerator ExportUserData()
    {
        UserLoaded = false;
        PlayerClass player = null;
        var loadData = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).GetValueAsync();
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
        });
        yield return new WaitUntil(() => player is not null && loadData.IsCompleted);
        PlayerData.PlayerClass = player;
        UserLoaded = true;
    }
    
    public static void PushMap(string level)
    {
        Debug.Log("Почта " + AuthManager.User.Email);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).Child("Map").SetRawJsonValueAsync(level);
    }

    public static void PushUserData(PlayerClass playerClass)
    {
        var userJson = JsonConvert.SerializeObject(playerClass);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthManager.User.DisplayName).SetRawJsonValueAsync(userJson);
    }

    public static void PushRank()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Leaderboard").Child(AuthManager.User.DisplayName).SetRawJsonValueAsync("0"); //todo тут пушим ранг, 0 заменить на переменную
    }

}

