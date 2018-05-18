using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class chatTest : MonoBehaviour {

    public DatabaseReference reference;
    public string userID;
    public int chatroomID;

    void Start () {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze moeten we later ophalen
        userID = "123456"; // auth.CurrentUser.UserId;
        chatroomID = 1; // Hier een chatroomID gebruiken, deze staan bij de user tabel, staat deze chatroomID bij de gebruiker zodat hij niet zomaar 1 opend?

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        sendMessage(userID,"Bericht inhoud"); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is
    }

    // Update is called once per frame
    void Update () {
		
	}

    void sendMessage(string from, string content)
    {
        chatMessage Message = new chatMessage(from, content);
        string json = JsonUtility.ToJson(Message);
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json); // userID vervangen met het daadwerkelijke userID van de gebruiker
    }

}

public class chatMessage
{
    public string user;
    public string content;
    public string date;

    public chatMessage(string from, string content)
    {
        this.user = from;
        this.content = content;
        this.date = System.DateTime.UtcNow.ToString();
    }
}
