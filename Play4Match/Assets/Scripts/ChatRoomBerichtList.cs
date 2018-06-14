using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomBerichtList : MonoBehaviour {

    public string date, message, name, chatroomID, PhotoUrl, ID, time;
    public ChatRoomBerichtList(string date, string message, string name, string chatroomID, string PhotoUrl, string ID)
    {
        this.date = date;
        this.message = message;
        this.name = name;
        this.chatroomID = chatroomID;
        this.PhotoUrl = PhotoUrl;
        this.ID = ID;
    }

}
