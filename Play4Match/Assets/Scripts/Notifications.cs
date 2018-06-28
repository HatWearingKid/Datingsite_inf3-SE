using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Notifications : MonoBehaviour {

    WWW www;
    private JSONNode JsonData;
	string userId;
	
	public GameObject soundEngine;
	
	public GameObject canvas;
	public GameObject notification;
	
	// Use this for initialization
	void Start () {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		userId = auth.CurrentUser.UserId;

		// Repeat method every 5 seconds
		InvokeRepeating("GetNotifications", 5, 5);
	}
	
	// Create notification ask a string. 
	void CreateNotification(string message)
	{
		GameObject newNotification = Instantiate(notification, canvas.transform);
		
		newNotification.GetComponentInChildren<Text>().text = message;
		newNotification.SetActive(true);
		soundEngine.GetComponent<SoundScript>().PlaySound("notificationSound");
	}
	
	void GetNotifications()
	{
		FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userId).Child("Notifications").GetValueAsync().ContinueWith(
		task => {
			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;

				foreach (var childSnapshot in snapshot.Children)
				{
					CreateNotification(childSnapshot.Value.ToString());
				}
				
				DatabaseReference dbref = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(userId).Child("Notifications");
				dbref.RemoveValueAsync();
			}
		});
		
		
		
	}
}
