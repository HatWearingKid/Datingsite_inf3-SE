using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class getQuestions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	  Firebase.Database.FirebaseDatabase dbInstance = Firebase.Database.FirebaseDatabase.DefaultInstance;
	  dbInstance.GetReference("Users").GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
				}
				else if (task.IsCompleted) {
				  DataSnapshot snapshot = task.Result;

				  foreach ( DataSnapshot question in snapshot.Children)
                  {
					Debug.Log (question);
				  }
				}
	  });
	}
	
	// Update is called once per frame
	//void Update () {
	//	//
	//}
}
