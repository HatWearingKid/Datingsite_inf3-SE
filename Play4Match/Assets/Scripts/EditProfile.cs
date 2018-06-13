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
	DatabaseReference locRef;

	private string name;
	private string gender;
	private string description;

	private string day;
	private string month;
	private string year;

	private string genderPref;
	private string minAge;
	private string maxAge;
	private int distance;

	void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

		if(user != null)
		{
			// Get the reference to the Users root node of the database.
			userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(user.UserId);
			// Get the reference to the Preference root node of the database.
			prefRef = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(user.UserId).Child("Preferences");
			// Get the reference to the Location root node of the database.
			locRef = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(user.UserId).Child("Location");
		}
	}

    #region Getters & Setters
    public void SetName(string _name){
		name = _name;
	}
		
	public void GetName(InputField input)
	{
		name = input.text;
	}
		
	public void GetGender(Dropdown dropdown)
	{
		gender = dropdown.options[dropdown.value].text;
	}

    public void GetDay(InputField input)
    {
        day = input.text;
    }

    public void SetDay(string _day)
    {
        day = _day;
    }

    public void GetMonth(InputField input)
    {
        day = input.text;
    }

    public void SetMonth(string _month)
    {
        month = _month;
    }

    public void GetYear(InputField input)
    {
        year = input.text;
    }

    public void SetYear(string _year)
    {
        year = _year;
    }

    public void GetGenderPref(Dropdown dropdown)
	{
		genderPref = dropdown.options[dropdown.value].text;
	}

	public void GetMinAge(Slider slider)
	{
		minAge = slider.value.ToString();
	}

	public void GetMaxAge(Slider slider)
	{
        maxAge = (100 - slider.value + 18).ToString();
	}

    public void GetDistance(Dropdown dropdown)
    {
        if (dropdown.value == 0) {
            distance = 25;
        } else if (dropdown.value == 1) {
            distance = 50;
        } else if (dropdown.value == 2) {
            distance = 100;
        } else {
            distance = 0;
        }
    }

    public void SetDescription(string _description){
		description = _description;
	}

	public void GetDescription(InputField input)
	{
		description = input.text;
	}
    #endregion

    public void UpdateUser(){	
		// Update data in the Users root
		userRef.Child("Name").SetValueAsync(name);
		userRef.Child("Gender").SetValueAsync(gender);
		userRef.Child("DateOfBirth").SetValueAsync(day + "/" + month + "/" + year);
		userRef.Child("Description").SetValueAsync(description);
		userRef.Child("Distance").SetValueAsync(distance);

        // Update data in the Preferences root
        prefRef.Child("Gender").SetValueAsync(genderPref);
		prefRef.Child("AgeMin").SetValueAsync(minAge);
		prefRef.Child("AgeMax").SetValueAsync(maxAge);
	}
}