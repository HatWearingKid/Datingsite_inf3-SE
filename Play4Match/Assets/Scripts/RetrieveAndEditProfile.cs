using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class RetrieveAndEditProfile : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	string userID;

	void Start () {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void RetrieveProfile(){
		if (user != null) {
			userID = user.UserId;
			Debug.Log (userID);
		} else {
			Debug.Log ("User not logged in.");
		}

		FirebaseDatabase.DefaultInstance
			.GetReference("Users")
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
						if(userID.Equals(user.Key)){
							Debug.Log("It matches.");
							Debug.Log(user.Key + " ---- " + userID);

							IDictionary dictUser = (IDictionary)user.Value;
							Debug.Log(dictUser["Name"]);
						}
					}
				}
			});
	}
}