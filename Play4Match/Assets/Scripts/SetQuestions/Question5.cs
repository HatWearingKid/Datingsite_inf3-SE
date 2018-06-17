using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question5 : MonoBehaviour
{

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference reference;

    public Slider slider;

    public void insertAnswer()
    {
        float distance;

        if (slider.value == 5)
        {
            distance = 0;
        }
        else
        {
            distance = slider.value * 25;
        }

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

        //connect to firebase and get userid
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("Users").Child(auth.CurrentUser.UserId).Child("Distance").SetValueAsync(distance);
    }
}
