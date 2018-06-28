using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject chatPanel;
    public GameObject textPrefab;
    public GameObject textPrefabUser;
    public Text partnerName;
    public Button backButton;
    public TMP_InputField chatBox;
    public DatabaseReference chatRef;
    public DatabaseReference reference;
    Boolean addMessage = true;
    public string userID;
    public string chatroomID;
    public string content;
    public string date;
    public string user;
    public bool chatroomFound = false;
    private TouchScreenKeyboard keyboard;
    List<ChatRoomBericht> ChatRoomBerichten = new List<ChatRoomBericht>();
    public GameObject chatReportPanel;
    // matchpanel > chatviewpanel > chatview
    public GameObject chatViewScroll; 
    public string otherUser;

    public Boolean messageExist = true;

    List<BerichtenLijst> BerichtenLijst = new List<BerichtenLijst>();
    private bool initialStart = true;

    void Start()
    {
        //Connect to Firebase
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //Get the user ID
        userID = auth.CurrentUser.UserId;
        initialStart = false;

        BerichtenLijst = null;
        BerichtenLijst = new List<BerichtenLijst>();

        //Connect to Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        keyboard = TouchScreenKeyboard.Open(chatBox.text, TouchScreenKeyboardType.Default);

        Button btn = backButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        scrollDown();

    }

    void OnEnable()
    {

        if (initialStart == false) // When enabled after reopening, not for the first load
        {
            foreach (Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject); // Delete all existing chats from the content
            }
            CancelInvoke("BuildChat");
            Start(); // Start the canvas like normal, because we dit reset everything
        }
    }

    public void scrollDown()
    {
        //Scroll to bottem of chat
        GameObject.Find("ChatView").GetComponent<ScrollRect>().verticalNormalizedPosition = -0.1f;
    }

    void Update()
    {   
        //Empty chatpanel when chat is changed
        if (chatroomID.ToString() != "" && chatroomFound == false)
        {
            foreach (Transform child in chatPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            chatRef = null;
            chatRef = FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(chatroomID.ToString());
            chatRef.ChildAdded += ChatChildAdded;
            chatroomFound = true;
            GetPartnerName();
            scrollDown();
        }

        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || (keyboard != null && keyboard.done))
            {
                if (chatroomFound == true)
                {
                    //Send user ID and text from inputfield
                    sendMessage(userID, chatBox.text);
                    chatBox.text = "";
                }
            }
        }

        
    }

    void OnDisable()
    {

        CancelInvoke("BuildChat");
        BerichtenLijst = null;
        BerichtenLijst = new List<BerichtenLijst>();
        foreach (Transform child in chatPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


public void SendMessageToChat(string text, string user)
    {
        if (addMessage = true)
        {
            //message from user
            if (userID == user)
            {
                GameObject newObjUser = (GameObject)Instantiate(textPrefabUser, chatPanel.transform);

                float sum = 400 - (text.Length * text.Length) + 50;

                //minimal chatmessage width
                if (sum < 100f)
                {
                    sum = 100f;
                }

                //maximal chatmessage width
                if (sum > 400f)
                {
                    sum = 400f;
                }

                //set textpanel position
                newObjUser.transform.Find("TextPanel").GetComponent<RectTransform>().offsetMin = new Vector2(sum, 0);
                //set text in message
                newObjUser.transform.Find("TextPanel").Find("Message").GetComponent<TextMeshProUGUI>().text = text;
            }
            else
            {
                //message from chat partner
                otherUser = user;
                if (user != "SYSTEEMBERICHT")
                {

                    GameObject newObjUser = (GameObject)Instantiate(textPrefab, chatPanel.transform);

                    float sum = 400 - (text.Length * text.Length) + 50;

                    //minimal chatmessage width
                    if (sum < 100f)
                    {
                        sum = 100f;
                    }
                    //maximal chatmessage width
                    if (sum > 400f)
                    {
                        sum = 400f;
                    }

                    //set textpanel position
                    newObjUser.transform.Find("TextPanel").GetComponent<RectTransform>().offsetMax = new Vector2((sum * -1), 0);
                    //set text in message
                    newObjUser.transform.Find("TextPanel").Find("Message").GetComponent<TextMeshProUGUI>().text = text;
                }


            }
            addMessage = true;
        }
    }
    //send message to firebase
    void sendMessage(string from, string content)
    {
        chatMessage2 Message = new chatMessage2(from, content);
        string json = JsonUtility.ToJson(Message);
        //give unique key to message
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        //send message to chat/chatroomID/...
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json);

        scrollDown();
    }



    public void GetPartnerName()
    {
        
        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userID).Child("Chatrooms").Child(chatroomID).GetValueAsync().ContinueWith(
               task => {
                   if (task.IsCompleted)
                   {
                       //take snapshot result
                       DataSnapshot snapshot = task.Result;
                       //take snapshot userID's
                       var user2_db = snapshot.Child("users").Value.ToString();
                       //split the two userID's
                       string[] users = user2_db.Split('|');
                       foreach (string user in users)
                       {
                           if (user != userID)
                           {
                               //get partner name
                               FirebaseDatabase.DefaultInstance.GetReference("Users").Child(user).GetValueAsync().ContinueWith(
                                                      task2 =>
                                                      {
                                                          if (task2.IsCompleted)
                                                          {
                                                              //take snapshot result
                                                              DataSnapshot snapshot2 = task2.Result;
                                                              IDictionary dictUser = (IDictionary)snapshot2.Value;
                                                              string name = dictUser["Name"].ToString();
                                                              //set partnername
                                                              partnerName.text = name;
                                                          }

                                                      });
                           }
                       }
                   }
               });
    }

    void ChatChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError == null)
        {
            //get content, date, user
            content = args.Snapshot.Child("content").Value.ToString();
            date = args.Snapshot.Child("date").Value.ToString();
            user = args.Snapshot.Child("user").Value.ToString();
            //put data in sendmessage
            //SendMessageToChat(content, user);
            addMessage = false;

            Boolean messageExist = false;
            for (int i = 0; i < BerichtenLijst.Count; i++)
            {
                if(BerichtenLijst[i].ID.ToString() == args.Snapshot.Key)
                {
                    messageExist = true;
                }
            }

            if (messageExist == false)
            {
                BerichtenLijst.Add(new BerichtenLijst(content, user, date, args.Snapshot.Key));
                Invoke("BuildChat", 1);
            }
            
        }
    }

    void BuildChat()
    {
        foreach (Transform child in chatPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < BerichtenLijst.Count; i++)
        {
            SendMessageToChat(BerichtenLijst[i].content.ToString(), BerichtenLijst[i].user.ToString());
        }

        
    }

    //empty chatroom ID on click
    void TaskOnClick()
    {
        chatroomID = "";
        chatroomFound = false;
    }

    //get time difference
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

    public void AddReport()
    {
        //report chat partner
        string type = "chatReport";
        string data = "";
        reportData report = new reportData(otherUser, userID, data);
        string json = JsonUtility.ToJson(report);
        reference.Child(type).Child(otherUser).SetRawJsonValueAsync(json);
        chatReportPanel.SetActive(true);
        Invoke("hideReport", 3f);

    }

    public void hideReport()
    {
        //hide report panel
        chatReportPanel.SetActive(false);
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

public class BerichtenLijst : MonoBehaviour
{

    public string content, user, date, ID;
    public BerichtenLijst(string content, string user, string date, string ID)
    {
        this.date = date;
        this.content = content;
        this.user = user;
        this.ID = ID;
    }

}