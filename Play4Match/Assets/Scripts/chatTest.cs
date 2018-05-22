using System.Collections;
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

    public bool message1 = false;

    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze moeten we later ophalen
        userID = "T2us9Y1uRnPfT0EoM4KMmQdMzvj2"; // auth.CurrentUser.UserId;
        chatroomID = "1"; // Hier een chatroomID gebruiken, deze staan bij de user tabel, staat deze chatroomID bij de gebruiker zodat hij niet zomaar 1 opend?

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //sendMessage(userID, "Bericht inhoud"); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is

        chatRef = FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString());
        chatRef.ChildAdded += ChatChildAdded;

        createChatroom("T2us9Y1uRnPfT0EoM4KMmQdMzvj2", "uUCL98DeyubpwlGgZfS6CCgNynJ2"); // Hardcoded nog even de 2 users waar de chat tussen plaatsvind, er word nu standaard telkens een nieuwe room gemaakt
        // Eigen user, andere user

        
    }

    // Update is called once per frame
    void Update()
    {

        // Tijdelijk
        if (chatroomID != "1" && message1 == false)
        {
            message1 = true;
            sendMessage(userID, "Bericht inhoud"); // Op basis van de chatroomID
        }

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
        }
    }


    void createChatroom(string user1, string user2)
    {
        // Een chat starten moet alleen mogelijk zijn als er nog geen chat is tussen beide users, anders hebben ze dubbele chatrooms met dezelfde user
        // Een chatroom tussen de users bestaat als een bepaald ID bij beide users in de Chatrooms staat
        
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

public class createChatroom
{
    public string key;
    public string users;

    public createChatroom(string key, string users)
    {
        this.key = key;
        this.users = users;

    }
}
