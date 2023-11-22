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
        LevelClass level = null;
        var loadLevel = FirebaseDatabase.DefaultInstance.RootReference.GetValueAsync();
        loadLevel.ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"Error {task.Exception}");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    var randomKey = GetRandomKey(snapshot);
                    Debug.Log(randomKey);
                    //File.WriteAllText(Application.persistentDataPath + "\\snapshot.json", JsonConvert.SerializeObject(snapshot.Child(randomKey).Value));
                    var levelJson = snapshot.Child(randomKey).GetRawJsonValue();
                    level = JsonConvert.DeserializeObject<LevelClass>(levelJson);
                    Debug.Log("Level loaded");
                }
            });
        yield return new WaitUntil(() => loadLevel.IsCompleted);

        LevelSaveManager.LoadedLevel = level;
        SceneManager.LoadScene("Level");
    }
    
    private static string GetRandomKey(DataSnapshot snapshot)
    {
        var keys = snapshot.Children.Select(child => child.Key).ToList();
        if (keys.Count <= 0) return null;
        var randomIndex = Random.Range(0, keys.Count);
        return keys[randomIndex];
    }
    
    public static void PushLevel(string level)
    {
        Debug.Log("Почта " + AuthManager.User.Email);
        FirebaseDatabase.DefaultInstance.RootReference.Child(AuthManager.auth.CurrentUser.DisplayName).SetRawJsonValueAsync(level);
    }
}

