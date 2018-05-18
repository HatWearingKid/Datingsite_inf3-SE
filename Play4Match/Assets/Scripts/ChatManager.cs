using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class ChatManager : MonoBehaviour {

    public string username;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    DatabaseReference mDatabaseRef;

    [SerializeField]
    List<Message> messagelist = new List<Message>();

    void Start() {
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Update() {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + chatBox.text);
                sendMessage(username, "test", chatBox.text); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is
                chatBox.text = "";


               
            }
        }
        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("You pressed space");
            }
        }
    }


    public void SendMessageToChat(string text)
    {
        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        messagelist.Add(newMessage);
    }

    void sendMessage(string from, string to, string content)
    {
        Debug.Log("Begin sendMessage");
        chatMessage Message = new chatMessage(from, to, content);
        string json = JsonUtility.ToJson(Message);

        Debug.Log(json);

        string key = mDatabaseRef.Child("Chat").Push().Key;


        mDatabaseRef.Child("Chat").Child(key).SetRawJsonValueAsync(json); // userID vervangen met het daadwerkelijke userID van de gebruiker
        Debug.Log("Einde sendMessage");

    }

}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;

    public Message()
    {

    }

}

public class chatMessage
{
    public string username;
    public string to;
    public string text;
    public string date;

    public chatMessage(string username, string to, string text)
    {
        ChatManager chatManager = new ChatManager();
        Debug.Log("Begin chatMessage");
        this.username = username;
        this.to = to;
        this.text = text;
        this.date = System.DateTime.UtcNow.ToString();
        Debug.Log("Einde chatMessage");
    }
}


