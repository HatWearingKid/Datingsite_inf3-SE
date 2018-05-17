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
	PlayerState pState = new PlayerState();

	void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		pState.playerName = "Piet";
		pState.lives = 3;
		pState.health = 0.8f;
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
			Debug.Log ("The passwords do not match.");
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
		//newUser = new User(userId);
		string json = JsonUtility.ToJson(pState);

		reference.Child("Gebruikers").Child("Test").Child("Name").SetRawJsonValueAsync(json);
		Debug.Log (json + "------" + pState + "------" + userId);
	}
}