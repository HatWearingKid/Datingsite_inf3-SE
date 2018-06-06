using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour {

    public string username; 

    public GameObject chatPanel;
    public GameObject textPrefab;
    public GameObject textPrefabUser;
    public Text partnerName;
    public Image profilePicture;
    public int paddingTop = 0;
    Boolean firstChatMessage = true;

    public TMP_Text textObject;
    public TMP_InputField chatBox;
    public DatabaseReference chatRef;
    public DatabaseReference reference;
    public string name;
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

    private string usersTabel = "Users"; // Na het testen "Users" gebruiken

    public string andereUser;


    [SerializeField]
    List<Message> messagelist = new List<Message>();

    void Start() {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Deze settings later ophalen van de Auth en welke match je aanklikt
        // TIJDELIJK: Maak 2 builds met deze 2 waardes omgedraait zodat ze met elkaar chatten
        andereUser = "AvPdwyvcvLYgs1YU6PTb6oWoVji3"; // Hardcoded user waarmee we chatten
        userID = "xh4S3DibGraTqCn8HascIIvdFR02"; // auth.CurrentUser.UserId


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        keyboard = TouchScreenKeyboard.Open(chatBox.text, TouchScreenKeyboardType.Default);

        getPartnerName();
        //addChatReport();

    }

    void Update() {
        if (chatroomID.ToString() != "" && chatroomFound == false)
        {
            chatRef = FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString());
            chatRef.ChildAdded += ChatChildAdded;
            chatroomFound = true;
        }

        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || (keyboard != null && keyboard.done))
            {
                if (chatroomFound == true)
                {
                    sendMessage(userID, chatBox.text); // Dit later ophalen uit de inputs en userID en ontvanger data die in de app bekend is
                    chatBox.text = "";

                }
                
               
            }
        }

        
    }


    public void SendMessageToChat(string text)
    {
		//System.Random rnd = new System.Random();
		//if (rnd.Next(0, 2) == 1)
		//{
		//    text = "kort bericht";
		//    userID = "xh4S3DibGraTqCn8HascIIvdFR02";

		//}
		//else
		//{
		//    text = "Een wat langer bericht met meer tekens";
		//    userID = "Bob";
		//}

		
        if (userID == "xh4S3DibGraTqCn8HascIIvdFR02")
        {
			GameObject newObjUser = (GameObject)Instantiate(textPrefabUser, chatPanel.transform);
			
			float sum = 400 - (text.Length * text.Length) + 50;

			if (sum < 100f)
			{
				sum = 100f;
			}

			if (sum > 400f)
			{
				sum = 400f;
			}

			newObjUser.transform.Find("TextPanel").GetComponent<RectTransform>().offsetMin = new Vector2(sum, 0);

			newObjUser.transform.Find("TextPanel").Find("Message").GetComponent<TextMeshProUGUI>().text = text;
		}
        else
        {
			GameObject newObjUser = (GameObject)Instantiate(textPrefab, chatPanel.transform);

			float sum = 400 - (text.Length * text.Length) + 50;

			if (sum < 100f)
			{
				sum = 100f;
			}

			if (sum > 400f)
			{
				sum = 400f;
			}

			newObjUser.transform.Find("TextPanel").GetComponent<RectTransform>().offsetMax = new Vector2((sum * -1), 0);

			newObjUser.transform.Find("TextPanel").Find("Message").GetComponent<TextMeshProUGUI>().text = text;
		}
    }

    void sendMessage(string from, string content)
    {
        chatMessage2 Message = new chatMessage2(from, content);
        string json = JsonUtility.ToJson(Message);
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json);
    }

    public void getPartnerName()
    {
        //string chatroomID = "-LDaU9iEIGxmT85YA9KZ";
        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userID).Child("Chatrooms").Child(chatroomID).GetValueAsync().ContinueWith(
               task => {
                   if (task.IsCompleted)
                   {
                       DataSnapshot snapshot = task.Result;

                       var user2_db = snapshot.Child("users").Value.ToString();

                       string[] users = user2_db.Split('|');
                       foreach (string user in users)
                       {
                           if (user != userID)
                           {
                               FirebaseDatabase.DefaultInstance.GetReference(usersTabel).Child(user).GetValueAsync().ContinueWith(
                                                      task2 =>
                                                      {
                                                          if (task2.IsCompleted)
                                                          {
                                                              DataSnapshot snapshot2 = task2.Result;
                                                              IDictionary dictUser = (IDictionary)snapshot2.Value;
                                                              name = dictUser["Name"].ToString();
                                                              //string photoUrl = dictUser["PhotoUrl"].ToString();
                                                              string photoUrl = "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + user + "%2FProfilePicture.png.jpg?alt=media";
                                                              Debug.Log("photoUrl: " + photoUrl);
                                                              StartCoroutine(LoadImg(photoUrl));
                                                              //Verander de header name naar chat partner name
                                                              partnerName.text = name;
                                                          }

                                                      });
                           }
                       }
                   }
               });
    }

    IEnumerator LoadImg(string avatarUrl)
    {
        WWW imgLink = new WWW(avatarUrl);

        while (!imgLink.isDone)
        {
            WaitForSeconds w;
            w = new WaitForSeconds(0.1f);
        }

        if (imgLink.isDone)
        {
            Texture2D texture = new Texture2D(imgLink.texture.width, imgLink.texture.height, TextureFormat.DXT1, false);
            imgLink.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

            profilePicture.GetComponent<Image>().sprite = spriteToUse;

            spriteToUse = null;
        }
        imgLink.Dispose();
        imgLink = null;

        yield return imgLink;
    }

    void ChatChildAdded(object sender, ChildChangedEventArgs args)
    {
        
        if (args.DatabaseError == null)
        {
             content = args.Snapshot.Child("content").Value.ToString();
             date = args.Snapshot.Child("date").Value.ToString();
             user = args.Snapshot.Child("user").Value.ToString();

            //if (user == userID)
            //{
            //    user = "Jij stuurde "; // Tekst rechts uitlijnen
            //} else
            //{
            //    user = "Je chatpartner stuurde "; // Tekst rechts uitlijnen
            //}

            //SendMessageToChat(user + " " + tijdVerschil(int.Parse(date)) + ":\n" + content);
			SendMessageToChat(content);
			// Dit tonen in de GUI
		}
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
        // Melding geven dat de chat is gereport
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
    public TMP_Text  textObject;

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

