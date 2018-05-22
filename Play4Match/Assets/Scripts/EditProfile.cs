using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class EditProfile : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	DatabaseReference reference;

	private string name;
	private string gender;
	private string dateOfBirth;
	private string country;

	void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
	}

	// Setter method for the emial
	public void SetName(string _name)
	{
		name = _name;
	}

	// Setter method for the password
	public void SetGender(string _gender)
	{
		gender = _gender;
	}

	public void SetDateOfBirth(string _dateOfBirth){
		dateOfBirth = _dateOfBirth;
	}

	public void SetCountry(string _country){
		country = _country;
	}

	public void UpdateUser(){	
		reference.Child ("Users").Child (user.UserId).Child("Name").SetValueAsync(name);
		reference.Child ("Users").Child (user.UserId).Child("Country").SetValueAsync(country);
		reference.Child ("Users").Child (user.UserId).Child("Gender").SetValueAsync(gender);
		reference.Child ("Users").Child (user.UserId).Child("DateOfBirth").SetValueAsync(dateOfBirth);
	}
}