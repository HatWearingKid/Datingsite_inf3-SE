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
    public int chatroomID;
    public List<string> recieved = new List<string>();

    public float refreshRate = 1f; // 1 seconde
    public bool updateLock = false;


    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze moeten we later ophalen
        userID = "123456"; // auth.CurrentUser.UserId;
        chatroomID = 1; // Hier een chatroomID gebruiken, deze staan bij de user tabel, staat deze chatroomID bij de gebruiker zodat hij niet zomaar 1 opend?

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        sendMessage(userID, "Bericht inhoud"); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is

        //getMessages();

    }

    // Update is called once per frame
    void Update()
    {

        refreshRate -= Time.deltaTime;
        if (refreshRate <= 0 && updateLock == false)
        {
            updateLock = true;

            getMessages();
            refreshRate = 1f;
            Debug.Log("Update check om: " + System.DateTime.UtcNow.ToString());

            updateLock = false;
        }
    }

    void sendMessage(string from, string content)
    {
        chatMessage2 Message = new chatMessage2(from, content);
        string json = JsonUtility.ToJson(Message);
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json); // userID vervangen met het daadwerkelijke userID van de gebruiker
    }

    void getMessages()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString()).GetValueAsync().ContinueWith(
                task => {
                    if (task.IsFaulted)
                    {

                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        foreach (var childSnapshot in snapshot.Children)
                        {
                            if (!recieved.Contains(childSnapshot.Key))
                            {
                                Debug.Log("Nieuw bericht gevonden om: " + System.DateTime.UtcNow.ToString());
                                var content = childSnapshot.Child("content").Value.ToString();
                                var date = childSnapshot.Child("date").Value.ToString();
                                var user = childSnapshot.Child("user").Value.ToString();

                                Debug.Log(date + " - " + user + " - " + content);
                                recieved.Add(childSnapshot.Key);
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
