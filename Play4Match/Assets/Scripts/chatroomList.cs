using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

public class chatroomList : MonoBehaviour
{

    public string username;

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public DatabaseReference chatRef;
    public DatabaseReference reference;
    public string userID;
    public string chatroomID;
    public string content;
    public string date;
    public string user;
    public bool chatroomFound = false;
    private TouchScreenKeyboard keyboard;
    public string lastMessage;
    public string lastMessageTime;
    List<ChatRoomBerichtList> ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();

    private string usersTabel = "Users"; // Na het testen "Users" gebruiken

    public string andereUser;

    public UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;

    [SerializeField]
    List<Message> messagelist = new List<Message>();

    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze settings later ophalen van de Auth en welke match je aanklikt
        // TIJDELIJK: Maak 2 builds met deze 2 waardes omgedraait zodat ze met elkaar chatten
        andereUser = "AvPdwyvcvLYgs1YU6PTb6oWoVji3"; // Hardcoded user waarmee we chatten
        userID = "xh4S3DibGraTqCn8HascIIvdFR02"; // auth.CurrentUser.UserId

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        getAllChatrooms(); // Alle chatRooms van user ophalen

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
        Debug.Log("Get all chatrooms");

        FirebaseDatabase.DefaultInstance.GetReference(usersTabel).Child(userID).Child("Chatrooms").GetValueAsync().ContinueWith(
                task => {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        foreach (var childSnapshot in snapshot.Children)
                        {
                            var user2_db = childSnapshot.Child("users").Value.ToString();
                            Debug.Log("users: " + user2_db);

                            string[] users = user2_db.Split('|');
                            foreach (string user in users)
                            {
                                if (user != userID)
                                {
                                    Debug.Log("Chat met user: " + user);
                                    FirebaseDatabase.DefaultInstance.GetReference(usersTabel).Child(user).GetValueAsync().ContinueWith(
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
                                                        Debug.Log("In het berichten gedeelte, er zijn " + snapshot3.ChildrenCount + " berichten in chatroomID " + childSnapshot.Key);

                                                        foreach (var childSnapshot3 in snapshot3.Children)
                                                        {
                                                            lastMessage = childSnapshot3.Child("content").Value.ToString();
                                                            lastMessageTime = childSnapshot3.Child("date").Value.ToString();
                                                        }

                                                        Debug.Log("ChatRoomBerichtenLijst count voor de add: " + ChatRoomBerichtenLijst.Count);
                                                        
                                                        ChatRoomBerichtenLijst.Add(
                                                            new ChatRoomBerichtList(
                                                                lastMessageTime.ToString(),
                                                                lastMessage.ToString(),
                                                                "hardcodedUser",
                                                                childSnapshot.Key.ToString()
                                                            )
                                                        ); // dictUser["Name"].toString()


                                                        /*
                                                        Debug.Log("ChatroomID: " + childSnapshot.Key.ToString());
                                                        Debug.Log("Date: " + lastMessageTime.ToString());
                                                        Debug.Log("message: " + lastMessage.ToString());
                                                        Debug.Log("name: " + dictUser["Name"]);
                                                        

                                                        ChatRoomBerichtenLijst.Add(new ChatRoomBerichtList(
                                                                "123456",
                                                                "lastMessage",
                                                                "user",
                                                                "chatroomID"));
                                                        Debug.Log("ChatRoomBerichtenLijst count voor na add: " + ChatRoomBerichtenLijst.Count);
                                                        */

                                                        if (ChatRoomBerichtenLijst.Count == snapshot.ChildrenCount)
                                                        {
                                                            Debug.Log("Ga naar buildChatroom, er zijn " + snapshot.ChildrenCount + " children");
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
        Debug.Log("In buildChatroom, rooms: " + ChatRoomBerichtenLijst.Count);
        //RectTransform parent = verticalLayoutGroup.GetComponent<RectTransform>();
        for (int i = 0; i < ChatRoomBerichtenLijst.Count; i++)
        {
            //GameObject g = new GameObject(ChatRoomBerichtenLijst[i].message);
            //UnityEngine.UI.Text t = g.AddComponent<UnityEngine.UI.Text>();
            //t.addComponent<RectTransform>().setParent(parent);
            //t.text = ChatRoomBerichtenLijst[i].message.ToString();

            Debug.Log("ChatroomID: " + ChatRoomBerichtenLijst[i].chatroomID.ToString() + "\n" +
                "Date: " + ChatRoomBerichtenLijst[i].date.ToString() + "\n" +
                "name: " + ChatRoomBerichtenLijst[i].name.ToString() + "\n" +
                "message: " + ChatRoomBerichtenLijst[i].message.ToString()
            );
        }

    }

}

public class ChatRoomBerichtList
{
    public string date;
    public string message;
    public string name;
    public string chatroomID;
    public ChatRoomBerichtList(string date, string message, string name, string chatroomID)
    {
        this.date = date;
        this.message = message;
        this.name = name;
        this.chatroomID = chatroomID;
    }
}


