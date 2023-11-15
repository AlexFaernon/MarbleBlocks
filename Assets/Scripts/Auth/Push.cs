using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using UnityEngine.Events;

public class Push : MonoBehaviour
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

    public void PushData()
    {
        var reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Почта " + AuthManager.User.Email);
        reference.Child(AuthManager.auth.CurrentUser.DisplayName).SetRawJsonValueAsync(Resources.Load("1").ToString());
    }
}

