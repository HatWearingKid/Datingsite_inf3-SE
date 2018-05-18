using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;

public class RegisterUser : MonoBehaviour {

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
	LikedBy likedBy = new LikedBy();
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
		
    public void setEmail(string _email)
    {
        email = _email;
    }

    public void setPassword(string _password)
    {
        password = _password;
    }

	public void setConfirmationPassword(string _confirmPassword)
	{
		confirmPassword = _confirmPassword;
	}

    public void Register()
    {
		if (password.Equals (confirmPassword)) {
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
				AddUser(user.UserId);
				SendEmail ();
				//auth.SignOut ();
			});
		} else {
			toast.MyShowToastMethod ("The passwords do not match.");
		}
    }

	public void SendEmail(){
		if (user != null) {
			user.SendEmailVerificationAsync ().ContinueWith (task => {
				if (task.IsCanceled) {
					Debug.LogError (task.Exception.InnerExceptions [0].Message);
					return;
				}
				if (task.IsFaulted) {
					Debug.LogError (task.Exception.InnerExceptions [0].Message);
					return;
				}

				toast.MyShowToastMethod ("A verification email has been sent, please activate your account.");
			});
		} else {
			toast.MyShowToastMethod ("Could not send email, something went wrong.");
		}
	}

    public void AddUser(string userId) {
		string jsonPlayer = JsonUtility.ToJson(player);
		string jsonLiked = JsonUtility.ToJson(liked);
		string jsonLikedBy = JsonUtility.ToJson(likedBy);
		string jsonPreferences = JsonUtility.ToJson(preferences);
		string jsonChatrooms = JsonUtility.ToJson(chatrooms);

		reference.Child("Gebruikers").Child(userId).SetRawJsonValueAsync(jsonPlayer);
		reference.Child("Gebruikers").Child(userId).Child("Liked").SetRawJsonValueAsync(jsonLiked);
		reference.Child("Gebruikers").Child(userId).Child("LikedBy").SetRawJsonValueAsync(jsonLikedBy);
		reference.Child("Gebruikers").Child(userId).Child("Preferences").SetRawJsonValueAsync(jsonPreferences);
		reference.Child("Gebruikers").Child(userId).Child("Chatrooms").SetRawJsonValueAsync(jsonChatrooms);
	}
}