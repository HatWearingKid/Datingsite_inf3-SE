using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using System.Linq;

public class getMatch : MonoBehaviour
{
    WWW www;
    private JSONNode JsonData;

    public GameObject matchButton;
	public GameObject matchButtonSpawns;

	public float startTime;
	public float UpdateTime;

	string[] spawnArray;
	List<string> spawnArraylist;
	List<string> userIdsSpawned;

	private void Start()
	{
		// Count the number of spawn points there are
		int maxSpawns = matchButtonSpawns.transform.childCount;
		spawnArray = new string[maxSpawns];

		for (int i = 0; i < maxSpawns; i++)
		{
			spawnArray[i] = (i + 1).ToString();
		}

		// Create list of spawn points
		spawnArraylist = new List<string>(spawnArray);

		userIdsSpawned = new List<string>();

		InvokeRepeating("GetMatch", startTime, UpdateTime);
	}

	public void GetMatch()
    {
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //string userid = "aBqlmQ6RewW7SMtuAwSvNke1cyp2";
        string userid = auth.CurrentUser.UserId;

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

    void CreateMatchButtons()
    {
		for (int i = 0; i < JsonData.Count; i++)
        {
			// If the UserId is not already spawned on the map
			if (userIdsSpawned.FirstOrDefault(s => s.Contains(JsonData[i]["Id"])) == null)
			{
				int randomIndex = Random.Range(0, spawnArraylist.Count);

				if (spawnArraylist[randomIndex] != null)
				{
					GameObject spawnObj = matchButtonSpawns.transform.Find(spawnArraylist[randomIndex]).gameObject;

					// Set new X and Z values from the spawnObj
					float newX = spawnObj.transform.position.x;
					float newZ = spawnObj.transform.position.z;

					// Create new matchbutton and fill it with values
					GameObject matchButtonNew = Instantiate(matchButton);
					matchButtonNew.name = "MatchButton" + spawnArraylist[randomIndex];

					matchButtonNew.transform.position = new Vector3(newX, matchButton.transform.position.y, newZ);
					matchButtonNew.GetComponent<CreateMatchPopup>().buttonName = matchButtonNew.name;
					matchButtonNew.GetComponent<CreateMatchPopup>().userId = JsonData[i]["Id"];
					matchButtonNew.GetComponent<CreateMatchPopup>().nameString = JsonData[i]["Name"] + " (" + JsonData[i]["Age"] + ")";
                    matchButtonNew.GetComponent<CreateMatchPopup>().locationString = JsonData[i]["Location"]["City"] + ", " + JsonData[i]["Location"]["CountryLong"];
                    matchButtonNew.GetComponent<CreateMatchPopup>().matchRateString = JsonData[i]["MatchRate"] + "%";
					matchButtonNew.GetComponent<CreateMatchPopup>().descriptionString = JsonData[i]["Description"];
					matchButtonNew.SetActive(true);

					// Add userId to list so we don't spawn it again
					userIdsSpawned.Add(JsonData[i]["Id"]);

					// Remove index from list to avoid spawning a matchbutton in that spot agian
					spawnArraylist.RemoveAt(randomIndex);
				}
			}
        }
    }

	public void AddSpawn(string spawnName)
	{
		spawnArraylist.Add(spawnName);
	}

	public void RemoveUserId(string userId)
	{
		for (int i = 0; i < userIdsSpawned.Count; i++)
		{
			// if the userId is found in the list
			if (userIdsSpawned[i].Equals(userId))
			{
				userIdsSpawned.RemoveAt(i);
			}
		}
	}
}
