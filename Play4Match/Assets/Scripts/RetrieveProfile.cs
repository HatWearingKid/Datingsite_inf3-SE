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
    string[] dateOfBirth;
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
		GetProfile();
	}

	void Update() {
		if (Time.fixedTime >= timer) {
			GetProfile();
			timer = Time.fixedTime + 2.0f;
		}
	}

    #region Retrieve data to set
    public void SetName(InputField input){
		if(node != null)
		{
			input.text = node["Name"];
		}	
	}

    public void SetDay(InputField input)
    {
        input.text = dateOfBirth[0];
    }

    public void SetMonth(InputField input)
    {
        input.text = dateOfBirth[1];
    }

    public void SetYear(InputField input)
    {
        input.text = dateOfBirth[2];
    }

    public void SetGender(Dropdown dropdown){
		if (node != null)
		{
			string gender = node["Gender"];
			if (gender.Equals("Male"))
			{
				dropdown.value = 0;
			}
			else if (gender.Equals("Female"))
			{
				dropdown.value = 1;
			}
			else
			{
				dropdown.value = 2;
			}
		}
	}

	public void SetGenderPref(Dropdown dropdown){
		if (node != null) {
			string gender = node ["Preferences"] ["Gender"];
			if (gender.Equals ("Male")) {
				dropdown.value = 0;
			} else if (gender.Equals ("Female")) {
				dropdown.value = 1;
			} else {
				dropdown.value = 2;
			}
		}
	}

	public void SetMinAge(Dropdown dropdown){
		if (node != null)
		{
			string minAge = node["Preferences"]["AgeMin"];
			dropdown.value = int.Parse(minAge) - 18;
		}
	}

	public void SetMaxAge(Dropdown dropdown){
		if (node != null)
		{
			string maxAge = node["Preferences"]["AgeMax"];
			dropdown.value = int.Parse(maxAge) - 18;
		}
	}

	public void SetDescription(InputField input){
		if(node != null)
		{
			input.text = node["Description"];
		}
	}

    public void SetDistance(Dropdown dropdown)
    {
        if (node != null)
        {
            int distance = node["Distance"];
            if (distance == 25)
            {
                dropdown.value = 0;
            } else if (distance == 50)
            {
                dropdown.value = 1;
            } else if (distance == 75)
            {
                dropdown.value = 2;
            } else
            {
                dropdown.value = 3;
            }
        }
    }
    #endregion

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
                            string birthdate = node["DateOfBirth"];
                            dateOfBirth = birthdate.Split('/');
                        }
					}
				}
			});
	}
}