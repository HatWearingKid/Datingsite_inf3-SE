using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;


/// <summary>
/// Login user. A class used to login to the game
/// </summary>
public class LoginUser : MonoBehaviour {
	// Initialize required variables
	private string email;
	private string password;

	public GameObject loadingScreen;

	Toast toast = new Toast();
	Firebase.Auth.FirebaseAuth auth;

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

	/*public void GoogleSignIn(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		Firebase.Auth.Credential credential =
			Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("SignInWithCredentialAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});
	}*/

	// Method to log the user in
	public void LoginUserOnClick(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		// A method from Firebase to login the user asynchrone
		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if (task.IsCanceled) {
				toast.MyShowToastMethod(task.Exception.InnerExceptions[0].Message);
				return;
			}
			if (task.IsFaulted) {
				toast.MyShowToastMethod(task.Exception.InnerExceptions[0].Message);
				return;
			}

			// If the user is logged in switch the scene
			Firebase.Auth.FirebaseUser newUser = task.Result;
			SwitchScene switschScene = new SwitchScene();
			switschScene.ChangeScene("sceneSemih");
		});
	}

	// Method to reset the password
	public void ResetPassword(){
		// Method from Firebase to reset the password asynchrone
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
			if (task.IsCanceled) {
				toast.MyShowToastMethod("1 - " + task.Exception.InnerExceptions[0].Message);
				return;
			}
			if (task.IsFaulted) {
				toast.MyShowToastMethod("2 - " + task.Exception.InnerExceptions[0].Message);
				return;
			}

			// Show a toast message when the email is sent.
			toast.MyShowToastMethod("Password reset email sent successfully.");
		});
	}
}