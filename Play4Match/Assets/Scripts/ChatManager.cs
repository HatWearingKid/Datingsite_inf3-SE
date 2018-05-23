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
    public DatabaseReference chatRef;
    public DatabaseReference reference;
    public string userID;
    public string chatroomID;
    public string content;
    public string date;
    public string user;
    public bool chatroomFound = false;
    private TouchScreenKeyboard keyboard;

    public string andereUser;

    [SerializeField]
    List<Message> messagelist = new List<Message>();

    void Start() {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze settings later ophalen van de Auth en welke match je aanklikt
        // TIJDELIJK: Maak 2 builds met deze 2 waardes omgedraait zodat ze met elkaar chatten
        andereUser = "T2us9Y1uRnPfT0EoM4KMmQdMzvj2"; // Hardcoded user waarmee we chatten
        userID = "testUser"; // auth.CurrentUser.UserId

        createChatroom(userID, andereUser); // Beide userID`s van de gebruikers, jezelf en de andere gebruiker

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // sendMessage(userID, "Bericht inhoud"); // userID, bericht (Roep altijd eerst createChatroom aan

        getAllChatrooms(); // Alle chatRooms van user ophalen [test]

        keyboard = TouchScreenKeyboard.Open(chatBox.text, TouchScreenKeyboardType.Default);

    }

    void Update() {
        if (chatBox.text != "")
        {
            //if (keyboard.status.Equals("Done"))
            //{
            //    sendMessage(username, "Keyboard status: Done"); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is
            //}
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || (keyboard != null && keyboard.done))
            { // TouchScreenKeyboard.Status.Done
                if (chatroomFound == true)
                {
                    sendMessage(username, chatBox.text); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is
                    chatBox.text = "";
                }
                
               
            }
        }

        if (chatroomID.ToString() != "" && chatroomFound == false)
        {
            chatRef = FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString());
            chatRef.ChildAdded += ChatChildAdded;
            chatroomFound = true;
            Debug.Log("ChatroomID gevonden, dus we kunnen nu berichten versturen");
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
            Debug.Log(args.Snapshot);

             content = args.Snapshot.Child("content").Value.ToString();
             date = args.Snapshot.Child("date").Value.ToString();
             user = args.Snapshot.Child("user").Value.ToString();

            SendMessageToChat(date + " - " + user + " - " + content);
            // Dit tonen in de GUI
        }
    }

    // Maak een chatroom aan
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
                            if ((user2_db == user1 + "|" + user2) || (user2_db == user2 + "|" + user1))
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

                            sendMessage(userID, "Chatroom aangemaakt, hier het 'Je hebt hetzelfde antwoord ingevuld als blabla op de volgende vraag: Is dit een vraag?'"); // Tijdelijk
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

                            Debug.Log("Chatroom tussen users: " + user2_db);

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

