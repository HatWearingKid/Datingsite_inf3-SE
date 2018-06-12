using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question2 : MonoBehaviour {

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference reference;

    public GameObject day;
	public GameObject month;
	public GameObject year;

	public GameObject minSliderText;
	public GameObject maxSliderText;

	public void insertAnswer()
    {
		string dayStr = day.GetComponent<Text>().text;
		string monthStr = month.GetComponent<Text>().text;
		string yearStr = year.GetComponent<Text>().text;

		string DateOfBirth = dayStr + "/" + monthStr + "/" + yearStr;

		string minAge = minSliderText.GetComponent<Text>().text;
		string maxAge = maxSliderText.GetComponent<Text>().text;

		Debug.Log(minAge + " " + maxAge);
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //string userId = "TestGebruiker";
        string userId = auth.CurrentUser.UserId;
            
        reference.Child("Users").Child(userId).Child("DateOfBirth").SetValueAsync(DateOfBirth);

		reference.Child("Users").Child(userId).Child("Preferences").Child("AgeMin").SetValueAsync(minAge);
		reference.Child("Users").Child(userId).Child("Preferences").Child("AgeMax").SetValueAsync(maxAge);
	}
}
