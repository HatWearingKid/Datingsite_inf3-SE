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
    public string username, content, date, user, lastMessage, lastMessageTime, userID, chatroomID;
    List<ChatRoomBerichtList> ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();
    public UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;
    public GameObject prefab, chatviewPanel, loadingScreen;
    private int chatroomNumber = 0;
    public int totalMessages = 0;

    private bool initialStart = true;
    private bool getChatroomsLock = false;

    void Start()
    {
        loadingScreen.SetActive(true); // Let the user know the app is loading
        initialStart = false;

        // Connect to the firebase database
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        userID = auth.CurrentUser.UserId; // Grab userID
        CancelInvoke("GetAllChatrooms"); // Cancel previous Invoke to prevent double invokes
        GetAllChatrooms(); // Load all chatrooms from matches, only for matches that exist on both sides
        loadingScreen.GetComponent<LoadingScreen>().fadeOut = true; // Remove loading screen when everything is done

    }

    void Update()
    {

    }

    void OnEnable()
    {
        if (initialStart == false) // When enabled after reopening, not for the first load
        {
            foreach (Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject); // Delete all existing chats from the content
            }
            CancelInvoke("GetAllChatrooms"); // Cancel previous Invoke to prevent double invokes
            Start(); // Start the canvas like normal, because we dit reset everything
        }
    }

    void OnDisable()
    {
        CancelInvoke("GetAllChatrooms"); // Cancel previous Invoke to prevent double invokes
    }


    public string TijdVerschil(int tijd) // Calculate the time past since the message was send
    {
        int huidigeTijd = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        int tijdVerschil = huidigeTijd - tijd;

        string result = "more then 1 year ago";

        float verschilMinuten = Mathf.Floor(tijdVerschil / 60); // Change seconds to minutes

        if (verschilMinuten < 524160)
        {
            result = Mathf.Floor(verschilMinuten / 40320) + " months ago";
            if (verschilMinuten < 80640)
            {
                result = "1 month ago";
            }
        }

        if (verschilMinuten < 40320)
        {
            result = Mathf.Floor(verschilMinuten / 10080) + " weeks ago";
            if (verschilMinuten < 20160)
            {
                result = "1 week ago";
            }
        }

        if (verschilMinuten < 10080)
        {
            result = Mathf.Floor(verschilMinuten / 1440) + " days ago";
            if (verschilMinuten < 2880)
            {
                result = "1 day ago";
            }
        }

        if (verschilMinuten < 1440)
        {
            result = Mathf.Floor(verschilMinuten / 60) + " hours ago";
            if (verschilMinuten < 120)
            {
                result = "1 hour ago";
            }
        }

        if (verschilMinuten < 60)
        {
            result = verschilMinuten + " minutes ago";
        }

        if (verschilMinuten < 1)
        {
            result = "Just now";
        }

        // Return the correct time past message
        return result;

    }

    public void ReleaseLock() // Release the lock on getAllChatrooms, to prevent it from running multiple times
    {
        getChatroomsLock = false;
        Invoke("GetAllChatrooms", 2); // Invoke after 2 seconds, since releaseLock is called immediately after we did get all the chatrooms
    }


    public void GetAllChatrooms() // Get all existing chatrooms from the current user
    {
        int messages = 0;
        if (getChatroomsLock == false) // Only when the function isnt locked
        {
            getChatroomsLock = true; // Set a lock as soon as this runs, to prevent is from running multiple times
            ChatRoomBerichtenLijst = null;
            ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>(); // Create/clear the chatRoomBerichtenLijst, containing all the chats, to pass to the buildChatrooms
            

            FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userID).Child("Chatrooms").GetValueAsync().ContinueWith( // Get all chatrooms found by the current user
                    task => {
                        if (task.IsCompleted)
                        {
                            DataSnapshot snapshot = task.Result;

                            if (snapshot.ChildrenCount == 0) { // No chatrooms found, do it will not continue, and release the lock so it will look again after the Invoke
                                ReleaseLock();
                            }

                            foreach (var childSnapshot in snapshot.Children) // Look through all the chats
                            {
                                var user2_db = childSnapshot.Child("users").Value.ToString(); // Find both users in this chatroom
                                string user_id_partner = "";

                                string[] users = user2_db.Split('|'); // Split the users to get both userID`s separately
                                foreach (string user in users)
                                {
                                    if (user != userID)
                                    {
                                        // the found userID isnt from the user itself
                                        user_id_partner = user;
                                    }
                                }

                                foreach (string user in users)
                                {

                                    if (user != userID)
                                    {
                                        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(user).GetValueAsync().ContinueWith( // Get all details from the other user, the partner of a specifik chat
                                        task2 => {
                                            if (task2.IsCompleted)
                                            {
                                                DataSnapshot snapshot2 = task2.Result;                                           

                                                if (snapshot2.ChildrenCount > 0) { // The chatpartner is found in the Users table
                                                FirebaseDatabase.DefaultInstance.GetReference("Chat").Child(childSnapshot.Key).GetValueAsync().ContinueWith(
                                                    task3 =>
                                                    {
                                                        if (task3.IsCompleted)
                                                        {
                                                            IDictionary dictUser = (IDictionary)snapshot2.Value; // Place all userInfo in the dictionary
                                                            DataSnapshot snapshot3 = task3.Result;
                                                            if (snapshot3.ChildrenCount == 0)
                                                            {
                                                                ReleaseLock();
                                                            }

                                                            int count = 0;
                                                            string user_tmp = "";
                                                            foreach (var childSnapshot3 in snapshot3.Children)
                                                            {
                                                                user_tmp = childSnapshot3.Child("user").Value.ToString();
                                                                if (user_tmp != "SYSTEEMBERICHT") // When it isnt a systemmessage
                                                                {
                                                                    lastMessage = childSnapshot3.Child("content").Value.ToString();
                                                                    lastMessageTime = childSnapshot3.Child("date").Value.ToString();
                                                                    // Get the content and time of the last message

                                                                }
                                                                else
                                                                {
                                                                    // Systemmessage, this isnt user at the moment, but needed for the chat to work
                                                                    lastMessage = "";
                                                                    lastMessageTime = childSnapshot3.Child("date").Value.ToString();
                                                                    count++;
                                                                    messages++; // count messages to see if something changed
                                                                }


                                                            }

                                                                // Add the message to the ChatRoomBerichtenLijst, to process later
                                                                ChatRoomBerichtenLijst.Add(
                                                                    new ChatRoomBerichtList(
                                                                        lastMessageTime.ToString(),
                                                                        lastMessage.ToString(),
                                                                        dictUser["Name"].ToString(),
                                                                        childSnapshot.Key.ToString(),
                                                                        "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + user_id_partner + "%2FProfilePicture.png?alt=media",
                                                                        user_tmp
                                                                    )
                                                                );

                                                            if (ChatRoomBerichtenLijst.Count == snapshot.ChildrenCount)
                                                            { // All the messages are loaded, so we can build on the canvas, only building this when needed
                                                              // Also used for later checks so it only rebuilds it when there are new messages

                                                                ReleaseLock();
                                                                if (totalMessages != messages)
                                                                { // Check if the amount of messages is different from the previous time
                                                                    ChatRoomBerichtenLijst.Sort((s1, s2) => s2.date.CompareTo(s1.date)); // Sort the messageslist on time

                                                                    totalMessages = messages;
                                                                    BuildChatroom(); // Build the GUI of all the messages
                                                                }

                                                            }

                                                        }


                                                    });
                                                } else
                                                {
                                                    // Chat from a user that is already deleted from the system, will add a placeholder message that wont be shown, but needed for the correct cound of messages
                                                        ChatRoomBerichtenLijst.Add(
                                                            new ChatRoomBerichtList(
                                                                "123456798".ToString(),
                                                                "DELETED".ToString(),
                                                                "DELETED".ToString(),
                                                                childSnapshot.Key.ToString(),
                                                                "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + userID + "%2FProfilePicture.png?alt=media",
                                                                "DELETED"
                                                            )
                                                        );
                                                    messages++;
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    });
            
        }
        getChatroomsLock = false;
        ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>();
        totalMessages = 0;
        // Clear all the fields

        loadingScreen.GetComponent<LoadingScreen>().fadeOut = true; // Remove loading screen


    }

    public void BuildChatroom()
    {
        loadingScreen.SetActive(true); // Show loadscreen when we are (re)building everything

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject); // Remove all the messages from the content
        }

        for (int i = 0; i < ChatRoomBerichtenLijst.Count; i++) // Loop through all the messages
        {
            if (ChatRoomBerichtenLijst[i].ID.ToString() != "DELETED")
            { // When it isnt a chat leftover from a deleted user
                GameObject newObj = (GameObject)Instantiate(prefab, transform); // Create object from prefab
                newObj.name = chatroomNumber.ToString(); // Set the name of the object, this is unique

                if (ChatRoomBerichtenLijst[i].ID.ToString() == "SYSTEEMBERICHT")
                { // Systemmessage, currently not used
                    newObj.transform.Find("naam").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].name.ToString();
                    newObj.transform.Find("time").GetComponent<Text>().text = "";

                } else if(ChatRoomBerichtenLijst[i].ID.ToString() != userID)
                { // Message from the chatpartner
                    newObj.transform.Find("naam").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].name.ToString() + " said ";
                    newObj.transform.Find("time").GetComponent<Text>().text = TijdVerschil(int.Parse(ChatRoomBerichtenLijst[i].date.ToString()));
                } else
                { // Message from your own user
                    newObj.transform.Find("naam").GetComponent<Text>().text = "You said ";
                    newObj.transform.Find("time").GetComponent<Text>().text = TijdVerschil(int.Parse(ChatRoomBerichtenLijst[i].date.ToString()));
                }

                newObj.transform.Find("bericht").GetComponent<Text>().text = ChatRoomBerichtenLijst[i].message.ToString(); // Set the message
                newObj.SetActive(true); // Make the prefab object active

                string chatroomID_TMP = ChatRoomBerichtenLijst[i].chatroomID.ToString();
                newObj.transform.Find("ActivateButton").GetComponent<Button>().onClick.AddListener(delegate { SetChatroomID(chatroomID_TMP); }); // Set the onclick off the button, and pass the chatroomID, so the chatmanager knows what chat to open

                string PhotoURL = ChatRoomBerichtenLijst[i].PhotoUrl.ToString(); // Get the Photo URL
                StartCoroutine(LoadImg(PhotoURL, newObj)); // Load the avatar in the background, to speed up the loading, so the user sees less of the loading screen
                chatroomNumber++; // Increate cound to go to next message
            }
        }
        ChatRoomBerichtenLijst = new List<ChatRoomBerichtList>(); // Clear the chatRoomBerichtenLijst
        loadingScreen.GetComponent<LoadingScreen>().fadeOut = true; // Remove loading screen

    }

    public void SetChatroomID(string data)
    {
        chatroomID = data;

        // Set chatroomid
        chatviewPanel.GetComponent<ChatManager>().chatroomID = data;

        // Set chatviewPanel active
        chatviewPanel.SetActive(true);
    }

    void AddReport(string who, string type, string data = "")
    {
        reportData report = new reportData(who, userID, data);
        string json = JsonUtility.ToJson(report);
        reference.Child(type).Child(who).Child(userID).SetRawJsonValueAsync(json);
        // The chat is reported
    }

    void SendMessage(string from, string content)
    { // Add the message to the database
        chatMessage2 Message = new chatMessage2(from, content); // Put everything in a object to jsonify, and insert in the database
        string json = JsonUtility.ToJson(Message);
        string key = reference.Child("Chat").Child(chatroomID.ToString()).Push().Key;
        reference.Child("Chat").Child(chatroomID.ToString()).Child(key).SetRawJsonValueAsync(json);
    }

    IEnumerator LoadImg(string avatarUrl, GameObject gameobject)
    { // Load images in the background, and place them in the avatar slot off the passed through object
        WWW imgLink = new WWW(avatarUrl);

        while (!imgLink.isDone)
        { // Wait till the loading is done
            WaitForSeconds w;
            w = new WaitForSeconds(0.1f);
        }

        if (imgLink.isDone)
        { // When we are sure it is done, add the image as a sprite
            Texture2D texture = new Texture2D(imgLink.texture.width, imgLink.texture.height, TextureFormat.DXT1, false);
            imgLink.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

            gameobject.transform.Find("Avatar").GetComponent<Image>().sprite = spriteToUse;

            spriteToUse = null;
        }
        imgLink.Dispose();
        imgLink = null;
        // Dispose everything to clear memory and prevent ghosting of images

        yield return imgLink;
    }

}