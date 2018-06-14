using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uncrush : MonoBehaviour
{
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	DatabaseReference reference;

	public GameObject CrushObj;

	public string CrushId;


	// Use this for initialization
	public void removeCrush()
	{
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

		//string userId = "xh4S3DibGraTqCn8HascIIvdFR02";
		string userId = auth.CurrentUser.UserId;

		if (userId != null)
		{
			// Get the reference to the Users root node of the database.
			DatabaseReference likedRef = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(userId).Child("Liked");

			CrushObj.SetActive(false);

			likedRef.Child(CrushId).RemoveValueAsync();
		}
	}

	public void insertCrush()
	{
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

		//string userId = "xh4S3DibGraTqCn8HascIIvdFR02";
		string userId = auth.CurrentUser.UserId;

		// INSERT THE CRUSH INTO DB
		string key = reference.Child("Users").Child(userId).Child("Liked").Push().Key;
		reference.Child("Users").Child(userId).Child("Liked").Child(CrushId).SetValueAsync(Firebase.Database.ServerValue.Timestamp);

		CrushObj.SetActive(true);
	}
}
