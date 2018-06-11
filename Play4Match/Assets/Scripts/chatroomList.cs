using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;
using UnityEngine.EventSystems;

public class chatroomList : MonoBehaviour
{

    public DatabaseReference chatRef, reference;
    public string username, content, date, user, lastMessage, lastMessageTime;
    public string userID = "xh4S3DibGraTqCn8HascIIvdFR02"; // auth.CurrentUser.UserId
    List<ChatRoomBerichtList> ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();
    public UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;
    public static string chatroomID; // ID meegeven aan de chat
    public GameObject prefab, chatList, chatviewPanel, loadingScreen, Camera;
    private int chatroomNumber = 0;

    private bool initialStart = true;

    void Start()
    {
        loadingScreen.SetActive(true);
        initialStart = false;

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        userID = "xh4S3DibGraTqCn8HascIIvdFR02"; // auth.CurrentUser.UserId

        getAllChatrooms();
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        if (initialStart == false)
        {
            // Delete all messages in the content object
            foreach (Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            Start();
        }
    }


    public string tijdVerschil(int tijd)
    {
        int huidigeTijd = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        int tijdVerschil = huidigeTijd - tijd;

        string result = "meer dan 1 jaar geleden";

        float verschilMinuten = Mathf.Floor(tijdVerschil / 60);

        if (verschilMinuten < 524160)
        {
            result = Mathf.Floor(verschilMinuten/40320) + " maanden geleden";
            if (verschilMinuten < 80640)
            {
                result = "1 maand geleden";
            }
        }

        if (verschilMinuten < 40320)
        {
            result = Mathf.Floor(verschilMinuten/10080) + " weken geleden";
            if (verschilMinuten < 20160)
            {
                result = "1 week geleden";
            }
        }

        if (verschilMinuten < 10080)
        {
            result = Mathf.Floor(verschilMinuten/1440) + " dagen geleden";
            if (verschilMinuten < 2880)
            {
                result = "1 dag geleden";
            }
        }

        if (verschilMinuten < 1440)
        {
            result = Mathf.Floor(verschilMinuten /60) + " uren geleden";
            if (verschilMinuten < 120)
            {
                result = "1 uur geleden";
            }
        }

        if (verschilMinuten < 60)
        {
            result = verschilMinuten + " minuten geleden";
        }

        if (verschilMinuten < 0)
        {
            result = tijdVerschil + " seconden geleden";
        }

        return result;

    }


    public void getAllChatrooms()
    {
        Debug.Log("getAllChatrooms");
        ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();

        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userID).Child("Chatrooms").GetValueAsync().ContinueWith(
                task => {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;

                        foreach (var childSnapshot in snapshot.Children)
                        {
                            var user2_db = childSnapshot.Child("users").Value.ToString();

                            string[] users = user2_db.Split('|');
                            foreach (string user in users)
                            {
                                
                                if (user != userID)
                                {
                                    FirebaseDatabase.DefaultInstance.GetReference("Users").Child(user).GetValueAsync().ContinueWith(
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

                                                        int count = 0;
                                                        foreach (var childSnapshot3 in snapshot3.Children) 
                                                        {
                                                            lastMessage = childSnapshot3.Child("content").Value.ToString();
                                                            lastMessageTime = childSnapshot3.Child("date").Value.ToString();
                                                            count++;
                                                        }

                                                        if(count > 0)
                                                        {

                                                            ChatRoomBerichtenLijst.Add(
                                                                new ChatRoomBerichtList(
                                                                    lastMessageTime.ToString(),
                                                                    lastMessage.ToString(),
                                                                    dictUser["Name"].ToString(),
                                                                    childSnapshot.Key.ToString(),
                                                                    "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + user + "%2FProfilePicture.png.jpg?alt=media",
                                                                    user
                                                                )
                                                            );
                                                        } else
                                                        {

                                                            ChatRoomBerichtenLijst.Add(
                                                                new ChatRoomBerichtList(
                                                                    (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString(),
                                                                    "Er is nog niets gezegd",
                                                                    dictUser["Name"].ToString(),
                                                                    childSnapshot.Key.ToString(),
                                                                    "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + user + "%2FProfilePicture.png.jpg?alt=media",
                                                                    user
                                                                )
                                                            );
                                                        }


                                                        if (ChatRoomBerichtenLijst.Count == snapshot.ChildrenCount)
                                                        {
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

		loadingScreen.GetComponent<LoadingScreen>().fadeOut = true;
    }

    public void buildChatroom()
    {
        for (int i = 0; i < ChatRoomBerichtenLijst.Count; i++)
        {            
            GameObject newObj = (GameObject)Instantiate(prefab, transform);
            newObj.name = chatroomNumber.ToString();
            newObj.transform.Find("naam").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].name.ToString() + " zei " + tijdVerschil(int.Parse(ChatRoomBerichtenLijst[i].date.ToString()));
            newObj.transform.Find("bericht").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].message.ToString();
            newObj.SetActive(true);

            string chatroomID_TMP = ChatRoomBerichtenLijst[i].chatroomID.ToString();
            newObj.transform.Find("ActivateButton").GetComponent<Button>().onClick.AddListener(delegate { setChatroomID(chatroomID_TMP); });

            string PhotoURL = "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F"+ ChatRoomBerichtenLijst[i].ID.ToString() + "%2FProfilePicture.png.jpg?alt=media";
            // ProfilePicture.png.jpg moet veranderd worden in de default naam van de afbeelding

            StartCoroutine(LoadImg(PhotoURL, newObj));
            chatroomNumber++;
        }
        ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();

    }

    public void setChatroomID(string data)
    {
        chatroomID = data;

        // Set chatroomid
        Camera.GetComponent<ChatManager>().chatroomID = data;

        // Zet chatviewPanel actief
        chatviewPanel.SetActive(true);
    }

    void addReport(string who, string type, string data = "")
    {
        reportData report = new reportData(who, userID, data);
        string json = JsonUtility.ToJson(report);
        reference.Child(type).Child(who).Child(userID).SetRawJsonValueAsync(json);
        // Melding geven dat de chat is gereport
    }



    // Maak een chatroom aan, met een default message, mee te geven als parameter
    void createChatroom(string user1, string user2, string message = "Chatroom aangemaakt, hier het 'Je hebt hetzelfde antwoord ingevuld als blabla op de volgende vraag: Is dit een vraag?'")
    {
        string users = user1 + "|" + user2;

        bool chatBestaat = false;

        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(user1).Child("Chatrooms").GetValueAsync().ContinueWith(
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
                                chatroomID = childSnapshot.Key;
                                break;
                            }
                        }

                        if (chatBestaat == false)
                        {
                            string key = reference.Child("Chat").Push().Key;
                            createChatroom createChatroom = new createChatroom(key, users);
                            string json = JsonUtility.ToJson(createChatroom);

                            reference.Child("Users").Child(user1).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            reference.Child("Users").Child(user2).Child("Chatrooms").Child(key).SetRawJsonValueAsync(json);
                            chatroomID = key;

                            sendMessage(userID, message);

                        }

                    }
                });

    }


    void sendMessage(string from, string content)
    {
        chatMessage2 Message = new chatMessage2(from, content);
        string json = JsonUtility.ToJson(Message);
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json);
    }

    IEnumerator LoadImg(string avatarUrl, GameObject gameobject)
    {
        WWW imgLink = new WWW(avatarUrl);
        
        while (!imgLink.isDone)
        {
            WaitForSeconds w;
            w = new WaitForSeconds(0.1f);
        }

        if (imgLink.isDone){
            Texture2D texture = new Texture2D(imgLink.texture.width, imgLink.texture.height, TextureFormat.DXT1, false);
            imgLink.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

            gameobject.transform.Find("Avatar").GetComponent<Image>().sprite = spriteToUse;

            spriteToUse = null;
        }
        imgLink.Dispose();
        imgLink = null;

        yield return imgLink;
    }

}