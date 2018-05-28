using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class Hud : MonoBehaviour {
	
	WWW www;
    private JSONNode JsonData;
	
	public GameObject menuPanel;
	public Button menuButton;
	public Text name;
	public Text score;

	// Use this for initialization
	void Start () {
		menuButton.GetComponent<Button>();
		menuButton.onClick.AddListener(buttonClicked);
		
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";
        //string userid = auth.CurrentUser.UserId;

        string url = "http://play4match.com/api/getProfile.php?id=" + userid;
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

            fillData();
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
	void Update () {
		
	}
	
	void fillData()
	{
		name.text = JsonData[0];
		score.text = "Score: " + JsonData[4];

	}
	
	void buttonClicked()
	{
		if(menuPanel.active)
		{
			menuPanel.SetActive(false);
		}
		else
		{
			menuPanel.SetActive(true);
		}
	}
}
