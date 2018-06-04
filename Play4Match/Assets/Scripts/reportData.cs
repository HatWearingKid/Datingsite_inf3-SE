using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reportData : MonoBehaviour {

    public string who, by, data;
    public reportData(string who, string by, string data)
    {
        this.who = who;
        this.by = by;
        this.data = data;
    }

}
