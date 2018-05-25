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

	public GameObject matchButtonSpawns;

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
		// Count the number of spawn points there are
		int maxSpawns = matchButtonSpawns.transform.childCount;

		string[] spawnArray = new string[maxSpawns];
		for(int i = 0; i < maxSpawns; i++)
		{
			spawnArray[i] = (i + 1).ToString();
		}

		List<string> spawnArraylist = new List<string>(spawnArray);
		spawnArray = null;

		for (int i = 0; i < JsonData.Count; i++)
        {
			int randomIndex = Random.Range(0, spawnArraylist.Count);

			if (spawnArraylist.Count > randomIndex && spawnArraylist[randomIndex] != null)
			{
				GameObject spawnObj = matchButtonSpawns.transform.Find(spawnArraylist[randomIndex]).gameObject;

				// Remove index from list to avoid spawning a matchbutton in that spot agian
				spawnArraylist.RemoveAt(randomIndex);

				// Set new X and Z values from the spawnObj
				float newX = spawnObj.transform.position.x;
				float newZ = spawnObj.transform.position.z;

				// Create new matchbutton and fill it with values
				GameObject matchButtonNew = Instantiate(matchButton);
				matchButtonNew.name = "MatchButton" + i;

				matchButtonNew.transform.position = new Vector3(newX, matchButton.transform.position.y, newZ);
				matchButtonNew.GetComponent<CreateMatchPopup>().buttonName = "MatchButton" + i;
				matchButtonNew.GetComponent<CreateMatchPopup>().userId = JsonData[i]["Id"];
				matchButtonNew.GetComponent<CreateMatchPopup>().nameString = JsonData[i]["Name"] + " (" + JsonData[i]["Age"] + ")";
				matchButtonNew.GetComponent<CreateMatchPopup>().matchRateString = JsonData[i]["MatchRate"] + "%";
				matchButtonNew.GetComponent<CreateMatchPopup>().descriptionString = JsonData[i]["Description"];
				matchButtonNew.SetActive(true);
			}
        }
    }
}
