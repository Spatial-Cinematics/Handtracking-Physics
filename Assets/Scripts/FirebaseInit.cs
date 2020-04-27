using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    
    public UnityEvent onFirebaseInitialized = new UnityEvent();
    
    private void Start() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Exception != null) {
                Debug.LogError($"Failed to initialized Firebase with {task.Exception}");
                return;
            }
            Debug.Log($"Firebase init");
            onFirebaseInitialized.Invoke();
        });
    }
}
