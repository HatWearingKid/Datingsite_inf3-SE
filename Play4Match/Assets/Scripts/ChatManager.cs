using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

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
    public string lastMessage;
    public string lastMessageTime;
    List<ChatRoomBericht> ChatRoomBerichten = new List<ChatRoomBericht>();

    private string usersTabel = "Gebruikers"; // Na het testen "Users" gebruiken

    public string andereUser;

    [SerializeField]
    List<Message> messagelist = new List<Message>();

    void Start() {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze settings later ophalen van de Auth en welke match je aanklikt
        // TIJDELIJK: Maak 2 builds met deze 2 waardes omgedraait zodat ze met elkaar chatten
        andereUser = "AvPdwyvcvLYgs1YU6PTb6oWoVji3"; // Hardcoded user waarmee we chatten
        userID = "xh4S3DibGraTqCn8HascIIvdFR02"; // auth.CurrentUser.UserId

        createChatroom(userID, andereUser); // Beide userID`s van de gebruikers, jezelf en de andere gebruiker

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // sendMessage(userID, "Bericht inhoud"); // userID, bericht (Roep altijd eerst createChatroom aan

        getAllChatrooms(); // Alle chatRooms van user ophalen [test]

        keyboard = TouchScreenKeyboard.Open(chatBox.text, TouchScreenKeyboardType.Default);

    }

    void Update() {
        if (chatroomID.ToString() != "" && chatroomFound == false)
        {
            chatRef = FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString());
            chatRef.ChildAdded += ChatChildAdded;
            chatroomFound = true;
            //Debug.Log("ChatroomID gevonden, dus we kunnen nu berichten versturen");
        }

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
                    sendMessage(userID, chatBox.text); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is
                    chatBox.text = "";
                } else
                {
                    chatBox.text = "Nog geen chatroomID gevonden";
                }
                
               
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
            //Debug.Log("Nieuw bericht gevonden om: " + System.DateTime.UtcNow.ToString());
            
            //Debug.Log(args.Snapshot);

             content = args.Snapshot.Child("content").Value.ToString();
             date = args.Snapshot.Child("date").Value.ToString();
             user = args.Snapshot.Child("user").Value.ToString();

            if (user == userID)
            {
                user = "Jij stuurde "; // Tekst rechts uitlijnen
            } else
            {
                user = "Je chatpartner stuurde "; // Tekst rechts uitlijnen
            }

            SendMessageToChat(user + " " + tijdVerschil(int.Parse(date)) + ":\n" + content);
            // Dit tonen in de GUI
        }
    }

    // Maak een chatroom aan
    void createChatroom(string user1, string user2)
    {
        string users = user1 + "|" + user2;

        bool chatBestaat = false;

        FirebaseDatabase.DefaultInstance.GetReference(usersTabel).Child(user1).Child("Chatrooms").GetValueAsync().ContinueWith(
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
                                //Debug.Log("Gebruik oude chatroomID weer: " + chatroomID);
                                break;
                            }
                        }

                        if (chatBestaat == false)
                        {
                            string key = reference.Child("Chat").Push().Key;
                            createChatroom createChatroom = new createChatroom(key, users);
                            string json = JsonUtility.ToJson(createChatroom);

                            reference.Child(usersTabel).Child(user1).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            reference.Child(usersTabel).Child(user2).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            chatroomID = key; // Zet de nieuwe chatroomID
                            //Debug.Log("Nieuwe chatroom aangemaakt: " + chatroomID);

                            sendMessage(userID, "Chatroom aangemaakt, hier het 'Je hebt hetzelfde antwoord ingevuld als blabla op de volgende vraag: Is dit een vraag?'"); // Tijdelijk
                        }

                    }
                });

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


    void getAllChatrooms() {

        FirebaseDatabase.DefaultInstance.GetReference(usersTabel).Child(userID).Child("Chatrooms").GetValueAsync().ContinueWith(
                task => {
                    if (task.IsCompleted) {
                        DataSnapshot snapshot = task.Result;

                        foreach (var childSnapshot in snapshot.Children) {
                            var user2_db = childSnapshot.Child("users").Value.ToString();

                            string[] users = user2_db.Split('|');
                            foreach (string user in users) {
                                if (user != userID) {

                                    FirebaseDatabase.DefaultInstance.GetReference(usersTabel).Child(user).GetValueAsync().ContinueWith(
                                    task2 => {
                                        if (task2.IsCompleted) {

                                            DataSnapshot snapshot2 = task2.Result;
                                            IDictionary dictUser = (IDictionary)snapshot2.Value;

                                            FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(childSnapshot.Key).GetValueAsync().ContinueWith(
                                                task3 => {
                                                    if (task3.IsCompleted) {

                                                        DataSnapshot snapshot3 = task3.Result;

                                                        foreach (var childSnapshot3 in snapshot3.Children) {
                                                            lastMessage = childSnapshot3.Child("content").Value.ToString();
                                                            lastMessageTime = childSnapshot3.Child("date").Value.ToString();
                                                        }

                                                        ChatRoomBerichten.Add(
                                                            new ChatRoomBericht(
                                                                lastMessageTime, 
                                                                lastMessage, 
                                                                dictUser["Name"].ToString(), 
                                                                childSnapshot.Key.ToString() 
                                                            )
                                                        );

                                                        if (ChatRoomBerichten.Count == snapshot.ChildrenCount)
                                                        {
                                                            ChatRoomBerichten.Sort((s1, s2) => s2.date.CompareTo(s1.date));
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
        // Hier later alles in de GUI laden
        for (int i = 0; i < ChatRoomBerichten.Count; i++)
        {
            Debug.Log("ChatroomID: " + ChatRoomBerichten[i].chatroomID.ToString() + "\n" +
                "Date: " + ChatRoomBerichten[i].date.ToString() + "\n" +
                "name: " + ChatRoomBerichten[i].name.ToString() + "\n" +
                "message: " + ChatRoomBerichten[i].message.ToString()
            );
        }
    }

    void addChatReport()
    {
        report reportChat = new report(userID);
        string json = JsonUtility.ToJson(reportChat);
        reference.Child("chatReport").Child(chatroomID.ToString()).Child(userID).SetRawJsonValueAsync(json);
    }
}

public class ChatRoomBericht
{
    public string date;
    public string message;
    public string name;
    public string chatroomID;
    public ChatRoomBericht(string date, string message, string name, string chatroomID)
    {
        this.date = date;
        this.message = message;
        this.name = name;
        this.chatroomID = chatroomID;
    }
}

public class report
{
    public string by;
    public report(string by)
    {
        this.by = by;
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
    public Int32 date;

    public chatMessage(string username, string to, string text)
    {
        //ChatManager chatManager = new ChatManager();
        Debug.Log("Begin chatMessage");
        this.username = username;
        this.to = to;
        this.text = text;
        this.date = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // System.DateTime.UtcNow.ToString();
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
    public Int32 date;

    public chatMessage2(string from, string content)
    {
        this.user = from;
        this.content = content;
        this.date = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // System.DateTime.UtcNow.ToString();
    }
}

