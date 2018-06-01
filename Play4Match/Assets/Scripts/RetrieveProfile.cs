using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using SimpleJSON;

/// <summary>
/// Retrieve and edit profile. Class to retrieve data and edit data of a user
/// </summary>
public class RetrieveProfile : MonoBehaviour {
	// Initialize required variables
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	string userID;
	Toast toast;
	float timer;
	JSONNode node;

	void Start () {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		toast = new Toast ();
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://play4matc.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		timer = Time.fixedTime + 2.0f;
		GetProfile ();
	}

	void FixedUpdate() {
		if (Time.fixedTime >= timer) {
			GetProfile ();
			timer = Time.fixedTime + 2.0f;
		}
	}

	public void SetName(InputField input){
		input.text = node["Name"];
	}

	public void SetDateOfBirth(InputField input){
		input.text = node["DateOfBirth"];
	}

	public void SetGender(Dropdown dropdown){
		string gender = node["Gender"];
		if (gender.Equals ("Male")) {
			dropdown.value = 0;
		} else if (gender.Equals ("Female")) {
			dropdown.value = 1;
		} else {
			dropdown.value = 2;
		}
	}

	public void SetGenderPref(Dropdown dropdown){
		string gender = node["Gender"];
		if (gender.Equals ("Male")) {
			dropdown.value = 0;
		} else if (gender.Equals ("Female")) {
			dropdown.value = 1;
		} else {
			dropdown.value = 2;
		}
	}

	public void SetMinAge(Dropdown dropdown){
		string minAge = node["Preferences"]["AgeMin"];
		dropdown.value = int.Parse (minAge) - 18;
	}

	public void SetMaxAge(Dropdown dropdown){
		string maxAge = node["Preferences"]["AgeMax"];
		dropdown.value = int.Parse (maxAge) - 18;
	}


	// Method to retrieve the user data
	public void GetProfile(){
		// Check if user is logged in
		if (user != null) {
			userID = user.UserId;
		}

		// Firebase method to make connection with the database and get the user's information
		FirebaseDatabase.DefaultInstance
			.GetReference("Users")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted)
				{
					toast.MyShowToastMethod(task.Exception.InnerExceptions [0].Message);
				}
				if(task.IsCanceled){
					toast.MyShowToastMethod(task.Exception.InnerExceptions [0].Message);
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
							node = JSON.Parse(user.GetRawJsonValue());
							Debug.Log(user.GetRawJsonValue());
						}
					}
				}
			});
	}
}