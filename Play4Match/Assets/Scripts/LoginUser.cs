using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUser : MonoBehaviour {
	private string email;
	private string password;

	public void setEmail(string _email)
	{
		email = _email;
	}

	public void setPassword(string _password)
	{
		password = _password;
	}

	public void LoginUserOnClick(){
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});

	}
}
