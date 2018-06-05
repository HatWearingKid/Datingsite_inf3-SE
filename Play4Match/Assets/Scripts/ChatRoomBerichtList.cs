using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomBerichtList : MonoBehaviour {

    public string date;
    public string message;
    public string name;
    public string chatroomID;
    public string PhotoUrl;
    public ChatRoomBerichtList(string date, string message, string name, string chatroomID, string PhotoUrl)
    {
        this.date = date;
        this.message = message;
        this.name = name;
        this.chatroomID = chatroomID;
        this.PhotoUrl = PhotoUrl;
    }

}
