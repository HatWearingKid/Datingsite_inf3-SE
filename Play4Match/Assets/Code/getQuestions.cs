using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class getQuestions : MonoBehaviour {


    WWW www;
    // Use this for initialization
    void Start () {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";//auth.CurrentUser.UserId; //forceert nu eelco's account anders moet ik elke keer inloggen om te testen

        string url = "http://play4match.com/api/getq.php?id=" + userid;
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
    void Update () {

	}

    public GameObject questiontext;
    
    public void ShowQuestion(int QuestionNumber)
    {
        //make question apear on screen
        //questiontext = GameObject.Find("Question").GetComponent<Text>();
        Debug.Log("question number: " + QuestionNumber);
        Debug.Log(www.text[QuestionNumber]);
    }
}
