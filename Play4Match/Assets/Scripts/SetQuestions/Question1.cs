using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question1 : MonoBehaviour {

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference reference;

    public GameObject text;

    public void insertAnswer()
    {
        string username = text.GetComponent<Text>().text;

        if (username.Length <= 25)
        {
            // Set up the Editor before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

            // Get the root reference location of the database.
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            //string userId = "TestGebruiker";
            string userId = auth.CurrentUser.UserId;
            
            reference.Child("Users").Child(userId).Child("Name").SetValueAsync(username);
        }
    }
}
