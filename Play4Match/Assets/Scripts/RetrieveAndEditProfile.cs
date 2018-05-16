using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class RetrieveAndEditProfile : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;

	void Start () {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void RetrieveProfile(){
		FirebaseDatabase.DefaultInstance
			.GetReference("Users/Name")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted)
				{
					Debug.Log(task.Exception.InnerExceptions [0].Message);
				}
				if(task.IsCanceled){
					Debug.Log(task.Exception.InnerExceptions [0].Message);
				}
				else if (task.IsCompleted)
				{
					DataSnapshot snapshot = task.Result;
					// Do something with snapshot...

					foreach (DataSnapshot user in snapshot.Children)
					{
						Debug.Log(user);
					}
				}
			});
	}
}