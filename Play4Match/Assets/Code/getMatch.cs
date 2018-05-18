using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class getMatch : MonoBehaviour
{
    WWW www;

    // Use this for initialization
    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";//auth.CurrentUser.UserId; //forceert nu eelco's account anders moet ik elke keer inloggen om te testen

        string url = "http://play4match.com/api/getmatch.php?id=" + userid;
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
