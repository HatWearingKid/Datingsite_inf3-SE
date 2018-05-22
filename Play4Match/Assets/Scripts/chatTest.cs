﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEditor;

public class chatTest : MonoBehaviour
{

    public DatabaseReference reference;
    public string userID;
    public string chatroomID;
    public DatabaseReference chatRef;

    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        userID = "T2us9Y1uRnPfT0EoM4KMmQdMzvj2"; // auth.CurrentUser.UserId;
        chatroomID = "1"; // word overschreven na aanroepen van createChatroom

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        chatRef = FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString());
        chatRef.ChildAdded += ChatChildAdded;

        createChatroom(userID, "AvPdwyvcvLYgs1YU6PTb6oWoVji2"); // Beide userID`s van de gebruikers, jezelf en de andere gebruiker
        // sendMessage(userID, "Bericht inhoud"); // userID, bericht (Roep altijd eerst createChatroom aan, deze maakt een room of haalt de oude room op)

        getAllChatrooms(); // Ophalen van een lijst met alle chatrooms van de gebruiker


    }

    // Update is called once per frame
    void Update()
    {

    }

    void sendMessage(string from, string content)
    {
            chatMessage2 Message = new chatMessage2(from, content);
            string json = JsonUtility.ToJson(Message);
            string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
            reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json);
    }

    void ChatChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError == null)
        {
                Debug.Log("Nieuw bericht gevonden om: " + System.DateTime.UtcNow.ToString());
                
                var content = args.Snapshot.Child("content").Value.ToString();
                var date = args.Snapshot.Child("date").Value.ToString();
                var user = args.Snapshot.Child("user").Value.ToString();

                Debug.Log(date + " - " + user + " - " + content);
                // Dit tonen in de GUI
        }
    }


    void createChatroom(string user1, string user2)
    {
        string users = user1 + "|" + user2;
        
        bool chatBestaat = false;

        FirebaseDatabase.DefaultInstance.GetReference("Gebruikers").Child(user1).Child("Chatrooms").GetValueAsync().ContinueWith(
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
                                if ((user2_db == user1+"|"+user2) || (user2_db == user2 + "|" + user1))
                                {
                                    chatBestaat = true;
                                    chatroomID = childSnapshot.Key; // Zet de oude chatroomID weer terug
                                    Debug.Log("Gebruik oude chatroomID weer: " + chatroomID);
                                    break;
                                }
                        }

                        if (chatBestaat == false)
                        {
                            string key = reference.Child("Chat").Push().Key;
                            createChatroom createChatroom = new createChatroom(key, users);
                            string json = JsonUtility.ToJson(createChatroom);

                            reference.Child("Gebruikers").Child(user1).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            reference.Child("Gebruikers").Child(user2).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            chatroomID = key; // Zet de nieuwe chatroomID
                            Debug.Log("Nieuwe chatroom aangemaakt: " + chatroomID);

                            sendMessage(userID, "Chatroom aangemaakt test bericht"); // Tijdelijk
                        }

                    }
                });
        
    }



    void getAllChatrooms()
    {

        FirebaseDatabase.DefaultInstance.GetReference("Gebruikers").Child(userID).Child("Chatrooms").GetValueAsync().ContinueWith(
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

                            string[] users = user2_db.Split('|');
                            foreach (string user in users)
                            {
                                if (user != userID)
                                {
                                    DatabaseReference chatGebruiker = FirebaseDatabase.DefaultInstance.GetReference("Gebruikers").Child(userID);
                                    DataSnapshot snapshot2 = task.Result;

                                    Debug.Log("Chat met " + snapshot2.Child("Name").Value.ToString() + " onder Chatroom ID: " + childSnapshot.Key);
                                }
                            }

                        }

                    }
                });

    }


}

public class chatMessage2
{
    public string user;
    public string content;
    public string date;

    public chatMessage2(string from, string content)
    {
        this.user = from;
        this.content = content;
        this.date = System.DateTime.UtcNow.ToString();
    }
}

public class createChatroom2
{
    public string key;
    public string users;

    public createChatroom2(string key, string users)
    {
        this.key = key;
        this.users = users;

    }
}
