using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class EditProfile : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	DatabaseReference userRef;
	DatabaseReference prefRef;

	private string name;
	private string gender;
	private string dateOfBirth;
	private string country;

	private string genderPref;
	private string minAge;
	private string maxAge;

	void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

		// Get the reference to the Users root node of the database.
		userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(user.UserId);
		// Get the reference to the Preference root node of the database.
		prefRef = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(user.UserId).Child("Preferences");
	}

	public void SetName(string _name){
		name = _name;
	}
		
	public void GetName(InputField input)
	{
		name = input.text;
	}
		
	public void GetGender(InputField input)
	{
		gender = input.text;
	}

	public void SetGender(string _gender){
		gender = _gender;
	}

	public void GetDateOfBirth(InputField input){
		dateOfBirth = input.text;
	}

	public void SetDateOfBirth(string _dateofbirth){
		dateOfBirth = _dateofbirth;
	}

	public void GetCountry(InputField input){
		country = input.text;
	}

	public void SetCountry(string _country){
		country = _country;
	}

	public void GetGenderPref(InputField input){
		genderPref = input.text;
	}

	public void SetGenderPref(string _genderPref){
		genderPref = _genderPref;
	}

	public void GetMinAge(InputField input){
		minAge = input.text;
	}

	public void SetMinAge(string _minAge){
		minAge = _minAge;
	}

	public void GetMaxAge(InputField input){
		maxAge = input.text;
	}

	public void SetMaxAge(string _maxAge){
		maxAge = _maxAge;
	}

	public void UpdateUser(){	
		// Update data in the Users root
		userRef.Child("Name").SetValueAsync(name);
		userRef.Child("Country").SetValueAsync(country);
		userRef.Child("Gender").SetValueAsync(gender);
		userRef.Child("DateOfBirth").SetValueAsync(dateOfBirth);

		// Update data in the Preferences root
		prefRef.Child("Gender").SetValueAsync(genderPref);
		prefRef.Child("AgeMin").SetValueAsync(minAge);
		prefRef.Child("AgeMax").SetValueAsync(maxAge);
	}
}