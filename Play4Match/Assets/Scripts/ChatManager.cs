using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;

public class ChatManager : MonoBehaviour {

    public string username;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color userMessage, info;

    [SerializeField]
    List<Message> messagelist = new List<Message>();

	void Start () {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        FirebaseApp.DefaultInstance.SetEditorP12FileName("YOUR-FIREBASE-APP-P12.p12");
        FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("SERVICE-ACCOUNT-ID@YOUR-FIREBASE-APP.iam.gserviceaccount.com");
        FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");
    }
}
	
	void Update () {
        if(chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + chatBox.text);
                chatBox.text = "";

                // Read the input field and push a new instance
                // of ChatMessage to the Firebase database
                FirebaseDatabase.getInstance()
                        .getReference()
                        .push()
                        .setValue(new ChatMessage(input.getText().toString(),
                                FirebaseAuth.getInstance()
                                        .getCurrentUser()
                                        .getDisplayName())
                        );
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

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        UnityEngine.Debug.Log("Message ID: " + e.Message.MessageId);
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

}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;

    private string messageText;
    private string messageUser;
    private string messageTime;

    public Message(string messageText, string messageUser)
    {
        this.messageText = messageText;
        this.messageUser = messageUser;

        // Initialize to current time
        messageTime = System.DateTime.UtcNow.ToString();

    }

    public Message()
    {

    }

    public string getMessageText()
    {
        return messageText;
    }

    public void setMessageText(string messageText)
    {
        this.messageText = messageText;
    }

    public string getMessageUser()
    {
        return messageUser;
    }

    public void setMessageUser(string messageUser)
    {
        this.messageUser = messageUser;
    }

    public string getMessageTime()
    {
        return messageTime;
    }

    public void setMessageTime(string messageTime)
    {
        this.messageTime = messageTime;
    }
}
