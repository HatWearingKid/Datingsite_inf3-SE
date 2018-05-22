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
	Toast toast;
	WWW www;
	float timer;

	void Start () {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		toast = new Toast ();
		timer = Time.fixedTime + 3.0f;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://play4matc.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		if (user.PhotoUrl != null) {
			www = new WWW (user.PhotoUrl.ToString ());
			StartCoroutine (GetImage (www));
		}
	}

	void FixedUpdate() {
		if (Time.fixedTime >= timer) {
			GetProfile ();
			timer = Time.fixedTime + 3.0f;
		}
	}

	public void SetName(InputField input){
		input.text = dictUser["Name"].ToString();
	}

	public void SetAge(InputField input){
		input.text = dictUser["DateOfBirth"].ToString();
	}

	public void SetGender(InputField input){
		input.text = dictUser["Gender"].ToString();
	}

	public void SetCountry(InputField input){
		input.text = dictUser["Country"].ToString();
	}

	public void SetImage(Button button){
		button.image.sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0, 0));
	}

	public IEnumerator GetImage(WWW www){
		yield return www;
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