using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;
using UnityEngine.EventSystems;

public class chatroomList : MonoBehaviour
{

    public DatabaseReference chatRef, reference;
    public string username, userID, content, date, user, lastMessage, lastMessageTime;
    List<ChatRoomBerichtList> ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();
    public UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;
    public GameObject prefab, chatList;
    public static string chatroomID; // ID meegeven aan de chat

    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        userID = "xh4S3DibGraTqCn8HascIIvdFR02"; // auth.CurrentUser.UserId
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        getAllChatrooms();
    }

    void Update()
    {

    }


    public string tijdVerschil(int tijd)
    {
        int huidigeTijd = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        int tijdVerschil = huidigeTijd - tijd;
        string result = "";

        if (tijdVerschil >= 18144000)
        {
            result = Mathf.Floor(tijdVerschil / 217728000) + " jaar geleden";
        }

        if (tijdVerschil < 18144000)
        {
            result = Mathf.Floor(tijdVerschil / 18144000) + " maand geleden";
        }

        if (tijdVerschil < 604800)
        {
            result = Mathf.Floor(tijdVerschil / 604800) + " dagen geleden";
        }

        if (tijdVerschil < 86400)
        {
            result = Mathf.Floor(tijdVerschil / 3600) + " uur geleden";
        }

        if (tijdVerschil < 3600)
        {
            result = Mathf.Floor(tijdVerschil / 60) + " minuten geleden";
        }

        if (tijdVerschil < 60)
        {
            result = tijdVerschil + " seconden geleden";
        }

        return result;

    }


    void getAllChatrooms()
    {

        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userID).Child("Chatrooms").GetValueAsync().ContinueWith(
                task => {
                    if (task.IsCompleted)
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
                                    FirebaseDatabase.DefaultInstance.GetReference("Users").Child(user).GetValueAsync().ContinueWith(
                                    task2 => {
                                        if (task2.IsCompleted)
                                        {
                                            
                                            DataSnapshot snapshot2 = task2.Result;
                                            IDictionary dictUser = (IDictionary)snapshot2.Value;

                                            FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(childSnapshot.Key).GetValueAsync().ContinueWith(
                                                task3 => {
                                                    if (task3.IsCompleted)
                                                    {
                                                        
                                                        DataSnapshot snapshot3 = task3.Result;

                                                        foreach (var childSnapshot3 in snapshot3.Children) 
                                                        {
                                                            lastMessage = childSnapshot3.Child("content").Value.ToString();
                                                            lastMessageTime = childSnapshot3.Child("date").Value.ToString();
                                                        }

                                                        ChatRoomBerichtenLijst.Add(
                                                            new ChatRoomBerichtList(
                                                                lastMessageTime.ToString(),
                                                                lastMessage.ToString(),
                                                                dictUser["Name"].ToString(),
                                                                childSnapshot.Key.ToString(),
                                                                dictUser["PhotoUrl"].ToString()
                                                            )
                                                        );

                                                        if (ChatRoomBerichtenLijst.Count == snapshot.ChildrenCount)
                                                        {
                                                            ChatRoomBerichtenLijst.Sort((s1, s2) => s2.date.CompareTo(s1.date));
                                                            buildChatroom();
                                                        }

                                                    }


                                                });
                                        }
                                    });
                                }
                            }
                        }
                    }
                });

    }

    public void buildChatroom()
    {
        for (int i = 0; i < ChatRoomBerichtenLijst.Count; i++)
        {            
            GameObject newObj = (GameObject)Instantiate(prefab, transform);
            newObj.transform.Find("naam").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].name.ToString() + " zei " + tijdVerschil(int.Parse(ChatRoomBerichtenLijst[i].date.ToString()));
            newObj.transform.Find("bericht").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].message.ToString();
            newObj.SetActive(true);

            string chatroomID_TMP = ChatRoomBerichtenLijst[i].chatroomID.ToString();
            newObj.transform.Find("ActivateButton").GetComponent<Button>().onClick.AddListener(delegate { setChatroomID(chatroomID_TMP); });
        }

    }

    public void setChatroomID(string data)
    {
        chatroomID = data;
        Debug.Log("chatroomID gezet op: " + chatroomID);
    }

    void addReport(string who, string type, string data = "")
    {
        reportData report = new reportData(who, userID, data);
        string json = JsonUtility.ToJson(report);
        reference.Child(type).Child(who).Child(userID).SetRawJsonValueAsync(json);
        // Melding geven dat de chat is gereport
    }



    // Maak een chatroom aan
    void createChatroom(string user1, string user2)
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

                            sendMessage(userID, "Chatroom aangemaakt, hier het 'Je hebt hetzelfde antwoord ingevuld als blabla op de volgende vraag: Is dit een vraag?'"); // Tijdelijk
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

}


public class ChatRoomBerichtList
{
    public string date;
    public string message;
    public string name;
    public string chatroomID;
    public string PhotoUrl;
    public ChatRoomBerichtList(string date, string message, string name, string chatroomID, string PhotoUrl)
    {
        this.date = date;
        this.message = message;
        this.name = name;
        this.chatroomID = chatroomID;
        this.PhotoUrl = PhotoUrl;
    }
}


public class reportData
{
    public string who, by, data;
    public reportData(string who, string by, string data)
    {
        this.who = who;
        this.by = by;
        this.data = data;
    }
}


