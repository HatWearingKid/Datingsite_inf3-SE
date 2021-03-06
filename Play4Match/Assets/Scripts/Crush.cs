﻿using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Crush : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	DatabaseReference reference;

	public GameObject Camera;

	public GameObject matchPanel;
	public GameObject matchButton;
	public GameObject reportButton;

	public static string chatroomID;

    public void InsertCrush()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		//connect to firebase and get userid
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		//string userId = "xh4S3DibGraTqCn8HascIIvdFR02";
		string userId = auth.CurrentUser.UserId;

        // INSERT THE CRUSH INTO DB
        string crushId = matchButton.GetComponent<CreateMatchPopup>().userId;

		reference.Child("Users").Child(userId).Child("Liked").Child(crushId).SetValueAsync(Firebase.Database.ServerValue.Timestamp);

        // Check if crush has liked you
         FirebaseDatabase.DefaultInstance.GetReference("Users").Child(crushId).Child("Liked").Child(userId).GetValueAsync().ContinueWith(
        task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Insert notification in other user
                string notification = "Someone has a crush on you!";

                // userId is found
                if (snapshot.Value != null)
                {
                    // Start chat
                    CreateChatroom(userId, crushId);

                    // Change notification message
                    notification = "You have found a match!";
                }

                // Insert notification
                string key = reference.Child("Users").Child(crushId).Child("Notifications").Push().Key;
                reference.Child("Users").Child(crushId).Child("Notifications").Child(key).SetValueAsync(notification);
            }
        });



        // Deactivate popup and remove matchbutton
        matchButton.GetComponent<CreateMatchPopup>().OnCrush();
		matchPanel.SetActive(false);

		// Re-add the MatchButton's spawn point for new matches
		Camera.GetComponent<getMatch>().AddSpawn(Regex.Replace(matchButton.name, "[^0-9.]", ""));
		Camera.GetComponent<getMatch>().RemoveUserId(crushId);
	}

    void CreateChatroom(string user1, string user2)
    {
        string users = user1 + "|" + user2;

        bool chatBestaat = false;

        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(user1).Child("Chatrooms").GetValueAsync().ContinueWith(
                task => {
                    if (task.IsFaulted)
                    {

                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        foreach (var childSnapshot in snapshot.Children)
                        {
                            var user2_db = childSnapshot.Child("users").Value.ToString();
                            if ((user2_db == user1 + "|" + user2) || (user2_db == user2 + "|" + user1))
                            {
                                chatBestaat = true;
                                chatroomID = childSnapshot.Key;
                                break;
                            }
                        }

                        if (chatBestaat == false)
                        {
                            string key = reference.Child("Chat").Push().Key;
                            createChatroom createChatroom = new createChatroom(key, users);
                            string json = JsonUtility.ToJson(createChatroom);

                            reference.Child("Users").Child(user1).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            reference.Child("Users").Child(user2).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            chatroomID = key;

                            sendMessage("SYSTEEMBERICHT", "Dit is een systeembericht");
                        }

                    }
                });

    }


    void sendMessage(string from, string content)
    {
        chatMessage2 Message = new chatMessage2(from, content);
        string json = JsonUtility.ToJson(Message);
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json);
    }

	public void ReportUser()
	{
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		//connect to firebase and get userid
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

		//string userId = "xh4S3DibGraTqCn8HascIIvdFR02";
		string userId = auth.CurrentUser.UserId;

		string crushId = matchButton.GetComponent<CreateMatchPopup>().userId;

		//Create new key
		string key = reference.Child("UserReport").Push().Key;

		//Push crushId and the userId that reported the user
		reference.Child("UserReport").Child(key).Child("UserId").SetValueAsync(crushId);
		reference.Child("UserReport").Child(key).Child("By").SetValueAsync(userId);
	}
}
