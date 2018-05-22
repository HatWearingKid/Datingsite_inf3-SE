using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class editProfile : MonoBehaviour {

    Firebase.Auth.FirebaseAuth auth;

    // Use this for initialization
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        getData();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void getData()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("Questions")
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...

                foreach (DataSnapshot question in snapshot.Children)
                {
                    Debug.Log(question);
                }
            }
        });
    }
}