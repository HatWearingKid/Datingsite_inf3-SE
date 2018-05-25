using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;

/// <summary>
/// Register user. A class used to register a user
/// </summary>
public class RegisterUser : MonoBehaviour {
	// Initialize required variables
    private string email;
    private string password;
	private string confirmPassword;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	DatabaseReference reference;
	Toast toast = new Toast();

	// Initialise player to insert into the DB
	Player player = new Player();
	Liked liked = new Liked();
	Preferences preferences = new Preferences();
	Chatrooms chatrooms = new Chatrooms();

    void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
	}
		
	// Setter method for the emial
    public void setEmail(string _email)
    {
        email = _email;
    }

	// Setter method for the password
    public void setPassword(string _password)
    {
        password = _password;
    }

	// Setter method for the confirmation password
	public void setConfirmationPassword(string _confirmPassword)
	{
		confirmPassword = _confirmPassword;
	}

	// Method to register the user
    public void Register()
    {
		// Check if both passwords match
		if (password.Equals (confirmPassword)) {
			// A method from Firebase to register the user asynchrone
			auth.CreateUserWithEmailAndPasswordAsync (email, password).ContinueWith (task => {
				if (task.IsCanceled) {
					toast.MyShowToastMethod (task.Exception.InnerExceptions [0].Message);
					return;
				}
				if (task.IsFaulted) {
					toast.MyShowToastMethod (task.Exception.InnerExceptions [0].Message);
					return;
				}

				// Firebase user has been created.
				user = task.Result;
				// Ass the user to the database
				AddUser(user.UserId);
				// Send an email to the user
				SendEmail ();
				// Log the user out (Firebase automatically logs the user in after registration)
				auth.SignOut ();
			});
		} else {
			// Show toast message if passwords do not match
			toast.MyShowToastMethod ("The passwords do not match.");
		}
    }

	// Method for sending verification email 
	public void SendEmail(){
		// Check if there's a user to send a mail to
		if (user != null) {
			// A Firebase method to send a verification email asynchrone
			user.SendEmailVerificationAsync ().ContinueWith (task => {
				if (task.IsCanceled) {
					Debug.LogError (task.Exception.InnerExceptions [0].Message);
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError (task.Exception.InnerExceptions [0].Message);
					return;
				}

				// Toast message when succeed
				toast.MyShowToastMethod ("A verification email has been sent, please activate your account.");
			});
		} else {
			// Toast message when there's no user to send a mail to
			toast.MyShowToastMethod ("Could not send email, something went wrong.");
		}
	}

	// Method to add a user to the database
    public void AddUser(string userId) {
		string jsonPlayer = JsonUtility.ToJson(player);
		string jsonLiked = JsonUtility.ToJson(liked);
		string jsonPreferences = JsonUtility.ToJson(preferences);
		string jsonChatrooms = JsonUtility.ToJson(chatrooms);

		// Insert the user's basic data into the database
		reference.Child("Users").Child(userId).SetRawJsonValueAsync(jsonPlayer);

		// Insert the nested nodes into the databse
		reference.Child("Users").Child(userId).Child("Liked").SetRawJsonValueAsync(jsonLiked);
		reference.Child("Users").Child(userId).Child("Preferences").SetRawJsonValueAsync(jsonPreferences);
		reference.Child("Users").Child(userId).Child("Chatrooms").SetRawJsonValueAsync(jsonChatrooms);
	}
}