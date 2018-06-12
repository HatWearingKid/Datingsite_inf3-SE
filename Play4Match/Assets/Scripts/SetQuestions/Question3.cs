using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question3 : MonoBehaviour {

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference reference;

    public Dropdown gender;
    public Dropdown genderPref;

    public void insertAnswer()
    {
		string genderStr = gender.options[gender.value].text;
		string genderPrefStr = genderPref.options[genderPref.value].text;

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        string userId = "TestGebruiker";
        //string userId = auth.CurrentUser.UserId;
            
        reference.Child("Users").Child(userId).Child("Gender").SetValueAsync(genderStr);
		reference.Child("Users").Child(userId).Child("Preferences").Child("Gender").SetValueAsync(genderPrefStr);
	}
}
