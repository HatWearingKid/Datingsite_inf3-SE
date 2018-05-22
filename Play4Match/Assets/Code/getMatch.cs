using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class getMatch : MonoBehaviour
{
    WWW www;
    private JSONNode JsonData;

    // Use this for initialization
    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";//auth.CurrentUser.UserId; //forceert nu eelco's account anders moet ik elke keer inloggen om te testen

        string url = "http://play4match.com/api/getmatch.php?id=" + userid;
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));

        GameObject matchButton = GameObject.Find("MatchButton");

        matchButton.GetComponent<CreateMatchPopup>().nameString = "Test";
        matchButton.GetComponent<CreateMatchPopup>().ageString = "patat";
        matchButton.GetComponent<CreateMatchPopup>().genderString = "vette";
        matchButton.GetComponent<CreateMatchPopup>().matchRateString = "dikke";

        GameObject matchButton2 = Instantiate(matchButton);

        //matchButton2.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        matchButton2.GetComponent<CreateMatchPopup>().objName = "Matchbutton2";
        matchButton2.GetComponent<CreateMatchPopup>().nameString = "Dit";
        matchButton2.GetComponent<CreateMatchPopup>().ageString = "IS";
        matchButton2.GetComponent<CreateMatchPopup>().genderString = "WAT";
        matchButton2.GetComponent<CreateMatchPopup>().matchRateString = "ANDERS!";
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        //parse json to variable
        JsonData = JSON.Parse(www.text);

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
