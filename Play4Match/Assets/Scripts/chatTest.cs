using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class chatTest : MonoBehaviour {

    public DatabaseReference reference;

    void Start () {
        Debug.Log("Start");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("Voor sendMessage aanroepen");
        sendMessage("From","To","Content");
        Debug.Log("Na sendMessage aanroepen");
    }

    // Update is called once per frame
    void Update () {
		
	}

    void sendMessage(string from, string to, string content)
    {
        Debug.Log("Begin sendMessage");
        chatMessage Message = new chatMessage(from, to, content);
        string json = JsonUtility.ToJson(Message);

        Debug.Log(json);

        string key = reference.Child("Chat").Child("userID").Push().Key;


        reference.Child("Chat").Child("userID").Child(key).SetRawJsonValueAsync(json); // userID vervangen met het daadwerkelijke userID van de gebruiker
        Debug.Log("Einde sendMessage");
        
    }

    
}

public class chatMessage
{
    public string from;
    public string to;
    public string content;
    public string date;

    public chatMessage(string from, string to, string content)
    {
        Debug.Log("Begin chatMessage");
        this.from = from;
        this.to = to;
        this.content = content;
        this.date = System.DateTime.UtcNow.ToString();
        Debug.Log("Einde chatMessage");
    }
}

