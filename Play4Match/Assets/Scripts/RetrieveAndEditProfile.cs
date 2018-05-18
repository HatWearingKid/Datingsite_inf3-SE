using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

/// <summary>
/// Retrieve and edit profile. Class to retrieve data and edit data of a user
/// </summary>
public class RetrieveAndEditProfile : MonoBehaviour {
	// Initialize required variables
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	string userID;

	void Start () {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	// Method to retrieve the user data
	public void RetrieveProfile(){
		// Check if user is logged in
		if (user != null) {
			userID = user.UserId;
			Debug.Log (userID);
		} else {
			Debug.Log ("User not logged in.");
		}

		// Firebase method to make connection with the database and get the user's information
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
				// If task succeeds
				else if (task.IsCompleted)
				{
					DataSnapshot snapshot = task.Result;

					// Itterate through all the users
					foreach (DataSnapshot user in snapshot.Children)
					{
						// Check if user equals to the logged in user to retrieve correct data
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