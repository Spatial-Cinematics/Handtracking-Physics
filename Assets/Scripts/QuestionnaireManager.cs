using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Proyecto26;
using TMPro;
using UnityEngine;

public class QuestionnaireManager : MonoBehaviour {

    public enum AnswerChoice {
        StronglyDisagree, Disagree, Neutral, Agree, StronglyAgree
    }
    
    private string _sessionId = "";
    private SessionObject _sessionObject;
    
    private List<string> questions = new List<string> {
        "Interacting with very large objects like the table and walls is more realistic with hand-tracking than controllers.",
        "In most cases, trying to move an object from one spot to another is easier with hand-tracking than it is with controllers.",
        "Some objects are much easier to manipulate with hand-tracking than with controllers.",
        "Inversely, some objects are much easier to manipulate using controllers",
        "Handling smaller objects is easier with hand-tracking than with controllers.",
        "Handling medium/large objects is easier with hand-tracking than with controllers",
        "Handling objects with complex shapes is easier with hand-tracking than with controllers.",
        "Interacting with buttons, switches, and levers is easier using hand-tracking.",
        "Using hand-tracking is more satisfying than using controllers.",
        "Hand tracking interactions surpassed my expectations.",
        "I think I would favor hand-tracking relatively higher if the simulation was implemented better",
        "In the future, I hope to see hand-tracking become the norm for VR apps & games.",
        "Finished!\n\n Thanks for participating!\n\nSee the source code at https://github.com/Spatial-Cinematics"
    };

    private string[] info = {"This project is part of a study meant for testing hand-tracking physics!\n" ,
                          "Switch between hands and controllers at anytime - just put down/pickup the controller!\n" ,
                          "Please follow the prompts and the questions using the buttons on the wall.\n" ,
                          "Choose higher numbers for how much you agree with each statement (0 being strongly disagree, etc.)!\n" ,
                          "Please answer as truthfully as possible!\n" ,
                          "Thanks for participating!"};

    public int currentQuestionIndex = 0;
    public TMP_Text questionText, infoText;
    private FirebaseApp _firebaseApp;
    
    private void Start() {
        _sessionId = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        _sessionObject = new SessionObject(_sessionId);
        InitInfoText();
        SetQuestion(0);
    }
    public void SetQuestion(int index) {
        currentQuestionIndex = index;
        questionText.text = questions[index];
    }

    public void GiveAnswer(int answer) {
        
        if (currentQuestionIndex >= questions.Count)
            return;
#if !UNITY_EDITOR
        _sessionObject.answerData.answers.Add(((AnswerChoice)answer).ToString());
        if (_sessionObject.answerData.answers.Count >= questions.Count)
            RestClient.Post("https://hand-tracking-9a2c7.firebaseio.com/.json", _sessionObject);
#endif  
        currentQuestionIndex++;
        SetQuestion(currentQuestionIndex);
        
    }

    private void InitInfoText() {
        infoText.text = "";
        for (int i = 0; i < info.Length; i++) {
            infoText.text += $"{i+1}. {info[i]}\n";
        }
    }

    private void FireBaseSdkInit() { //
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp - a Firebase.FirebaseApp property of your application class.
                _firebaseApp = Firebase.FirebaseApp.DefaultInstance;
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInWithCustomTokenAsync("q9VfSsxv5hNP10nkT3293z6uzHN2").ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("Sign in canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("Sign in faulted: " + task.Exception);
            }
        });
    }
    
//    public void SaveData(int i) {
//        Debug.Log($"Data saved");
//        _database.GetReference(KEY_TEST).SetRawJsonValueAsync(JsonUtility.ToJson(testData));
//    }
    
}

[Serializable]
public class SessionObject {
    public string sessionId;
    public AnswerData answerData = new AnswerData();
    public SessionObject(string sessionId) {
        this.sessionId = sessionId;
    }
}

[Serializable]
public class AnswerData {
    public List<string> answers = new List<string>();
}

