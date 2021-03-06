﻿using System.Collections;
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
		
		toast.MyShowToastMethod("Trying to log in...");
		
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
			user = task.Result;
            if (user.IsEmailVerified) {
                toast.MyShowToastMethod("Logging in...");
                // Depending if the user has a completed profile or not, the scene will be loaded
                if (user.DisplayName == "1")
                {
                    SceneManager.LoadScene("scene0");
                } else
                {
                    SceneManager.LoadScene("startLevel");
                }
            } else
            {
                // If the user has not activated his mail, we inform the user
                toast.MyShowToastMethod("Please verify your email first.");
            }
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

    public void LogOut()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        SceneManager.LoadScene("main");
    }
}