using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chatMessage2 : MonoBehaviour {

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
