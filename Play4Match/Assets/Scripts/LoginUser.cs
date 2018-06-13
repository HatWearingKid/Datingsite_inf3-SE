using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using SimpleJSON;
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
	Firebase.Auth.FirebaseUser user;

    JSONNode node;
    SwitchScene switschScene = new SwitchScene();

    #region Set Email & Password
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
    #endregion

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
            SwitchScene(newUser.UserId);
		});
	}

	// Method to reset the password
	public void ResetPassword(){
		// Method from Firebase to reset the password asynchrone
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
			if (task.IsCanceled) {
				toast.MyShowToastMethod(task.Exception.InnerExceptions[0].Message);
				return;
			}
			if (task.IsFaulted) {
				toast.MyShowToastMethod(task.Exception.InnerExceptions[0].Message);
				return;
			}

			// Show a toast message when the email is sent.
			toast.MyShowToastMethod("Password reset email sent successfully.");
		});
	}

    public void SwitchScene(string userID)
    {
        // Firebase method to make connection with the database and get the user's information
        FirebaseDatabase.DefaultInstance
            .GetReference("Users")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    toast.MyShowToastMethod(task.Exception.InnerExceptions[0].Message);
                }
                if (task.IsCanceled)
                {
                    toast.MyShowToastMethod(task.Exception.InnerExceptions[0].Message);
                }
                // If task succeeds
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    // Itterate through all the users
                    foreach (DataSnapshot user in snapshot.Children)
                    {
                        // Check if user equals to the logged in user to retrieve correct data
                        if (userID.Equals(user.Key))
                        {
                            node = JSON.Parse(user.GetRawJsonValue());
                            if (node["CompleteProfile"] == true)
                            {
                                switschScene.ChangeScene("scene0");
                            } else
                            {
                                switschScene.ChangeScene("startLevel");
                            }
                        }
                    }
                }
            });
    }

    public void LogOut()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        SceneManager.LoadScene("main");
    }
}