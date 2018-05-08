﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUser : MonoBehaviour {
	private string email;
	private string password;
	public Text UserInfo;

	Firebase.Auth.FirebaseAuth auth;

	public void setEmail(string _email)
	{
		email = _email;
	}

	public void setPassword(string _password)
	{
		password = _password;
	}

	public void LoginUserOnClick(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("1 " + task.Exception.InnerExceptions[0].Message);
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("2 " + task.Exception.InnerExceptions[0].Message);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.Email, newUser.UserId);
			UserInfo.text = newUser.Email + " - Email verified: (" + newUser.IsEmailVerified + ")";
		}); 
		Debug.Log (email + "" + password);
	}
		
	public void SignOutUser(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.SignOut ();
	}
}