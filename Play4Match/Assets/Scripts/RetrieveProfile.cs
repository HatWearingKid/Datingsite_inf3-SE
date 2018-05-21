using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

/// <summary>
/// Retrieve and edit profile. Class to retrieve data and edit data of a user
/// </summary>
public class RetrieveProfile : MonoBehaviour {
	// Initialize required variables
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	string userID;
	IDictionary dictUser;
	Toast toast = new Toast();
	WWW www;

	void Start () {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
		GetProfile ();
	}

	public void SetName(InputField input){
		input.text = dictUser["Name"].ToString();
	}

	public void SetAge(InputField input){
		input.text = dictUser["dateOfBirth"].ToString();
	}

	public void SetGender(InputField input){
		input.text = dictUser["gender"].ToString();
	}

	public void SetCountry(InputField input){
		input.text = dictUser["Country"].ToString();
	}

	public void SetImage(Button button){
		if(user.PhotoUrl != null){
			GetImage ();
			button.image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		}
	}

	public IEnumerator GetImage(){
		if(user.PhotoUrl != null){
			www = new WWW(user.PhotoUrl.ToString());
			yield return www;
		}
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
							dictUser = (IDictionary)user.Value;
						}
					}
				}
			});
	}
}