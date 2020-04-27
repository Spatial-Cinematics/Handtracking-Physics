using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using UnityEngine;

public class QuestionaireManager : MonoBehaviour {

    private const string KEY_TEST = "TEST";
    private const string KEY_TEST_TWO = "TEST-2";
    private FirebaseDatabase _database;

    public TestData testData;
    
    private void Start() {
        _database = FirebaseDatabase.DefaultInstance;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveData();
    }

    public void SaveData() {
        Debug.Log($"Data saved");
        _database.GetReference(KEY_TEST).SetRawJsonValueAsync(JsonUtility.ToJson(testData));
        _database.GetReference(KEY_TEST_TWO).SetRawJsonValueAsync(JsonUtility.ToJson(testData));
    }
    
}

[Serializable]
public class TestData {
    private string testString1 = "this is a private string";
    public string testString2 = "this is a public string";
    public bool testBool = true;
}
