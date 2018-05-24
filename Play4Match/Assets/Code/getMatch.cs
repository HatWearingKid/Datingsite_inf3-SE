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

    public GameObject matchButton;

    // Use this for initialization
    void Start()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";
        //string userid = auth.CurrentUser.UserId;

        string url = "http://play4match.com/api/getmatch.php?id=" + userid;
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.isDone == true)
        {
            //parse json to variable
            JsonData = JSON.Parse(www.text);

            CreateMatchButtons();
        }

        // check for errors
        if (www.error == null)
        {
            //Debug.Log("WWW Ok!: " + www.text);
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

    void CreateMatchButtons()
    {
        for (int i = 0; i < JsonData.Count; i++)
        {
            GameObject matchButtonNew = Instantiate(matchButton);
            matchButtonNew.name = "MatchButton" + i;

            float newX = Random.Range(-250f, 1400f);
            float newZ = Random.Range(600f, 2200f);

            matchButtonNew.transform.position = new Vector3(newX, matchButton.transform.position.y, newZ);

            matchButtonNew.GetComponent<CreateMatchPopup>().buttonName = "MatchButton" + i;

            matchButtonNew.GetComponent<CreateMatchPopup>().userId = JsonData[i]["Id"];

            matchButtonNew.GetComponent<CreateMatchPopup>().nameString = JsonData[i]["Name"] + " (" + JsonData[i]["Age"] + ")";
            matchButtonNew.GetComponent<CreateMatchPopup>().matchRateString = JsonData[i]["MatchRate"] + "%";
            matchButtonNew.SetActive(true);
        }
    }
}
