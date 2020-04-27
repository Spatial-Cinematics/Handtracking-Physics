using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class QuestionaireManager : MonoBehaviour {

    public enum AnswerChoice {
        StronglyDisagree, Disagree, Neutral, Agree, StronglyAgree
    }
    
    private FirebaseDatabase _database;

    private string _sessionId;
    
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
        "In the future, I hope to see hand-tracking become the norm for VR apps & games."
    };

    private string[] info = {"This project is part of a study meant for testing hand-tracking physics!\n" ,
                          "Switch between hands and controllers at anytime - just put down/pickup the controller!" ,
                          "Please follow the prompts and the questions using the buttons on the wall.\n" ,
                          "Choose higher numbers for how much you agree with each statement (0 being strongly disagree, etc.)!\n" ,
                          "Please answer as truthfully as possible!\n" ,
                          "Thanks for participating!"};

    public int currentQuestionIndex;
    public TMP_Text questionText, infoText;
    
    private void Start() {
        _database = FirebaseDatabase.DefaultInstance;
        _sessionId = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        InitInfoText();
        SetQuestion(0);
    }
    public void SetQuestion(int index) {
        currentQuestionIndex = index;
        questionText.text = questions[index];
    }

    public void GiveAnswer(int answer) {
        _database.GetReference($"{_sessionId}/{questions[currentQuestionIndex]}")
            .SetValueAsync(((AnswerChoice)answer).ToString());
        currentQuestionIndex++;
        SetQuestion(currentQuestionIndex);
    }

    private void InitInfoText() {
        infoText.text = "";
        for (int i = 0; i < info.Length; i++) {
            infoText.text += $"{i}. {info[i]}";
        }
    }
    
//    public void SaveData(int i) {
//        Debug.Log($"Data saved");
//        _database.GetReference(KEY_TEST).SetRawJsonValueAsync(JsonUtility.ToJson(testData));
//    }
    
}

