using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

public class CreateCrushList : MonoBehaviour {
	public GameObject prefab;

	public GameObject CrushList;
	
	private bool initialStart = true;

	public GameObject loadingScreen;

	void Start()
	{
		loadingScreen.SetActive(true);


		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		string userId = "xh4S3DibGraTqCn8HascIIvdFR02";
		//string userId = auth.CurrentUser.UserId;

		FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userId).Child("Liked").GetValueAsync().ContinueWith(
		task => {
			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;

				foreach (var childSnapshot in snapshot.Children)
				{
					string crushId = childSnapshot.Key.ToString();
					long timestamp = (long)childSnapshot.Value;

					string dateText = getDateAgo(timestamp);

					if (crushId != userId)
					{
						FirebaseDatabase.DefaultInstance.GetReference("Users").Child(crushId).GetValueAsync().ContinueWith(
						task2 => {
							if (task2.IsCompleted)
							{
								DataSnapshot snapshot2 = task2.Result;

								string crushName = "";
								string crushAge = "";

								foreach (var childSnapshot2 in snapshot2.Children)
								{
									if(childSnapshot2.Key.ToString() == "Name")
									{
										crushName = childSnapshot2.Value.ToString();
									}

									if (childSnapshot2.Key.ToString() == "Age")
									{
										crushAge = childSnapshot2.Value.ToString();
									}
								}

								if(crushName != "" && crushAge != "")
								{
									GameObject newObj = (GameObject)Instantiate(prefab, transform);
									newObj.transform.Find("NameAgeText").GetComponent<Text>().text = crushName + " (" + crushAge + ")";
									newObj.transform.Find("LocationText").GetComponent<Text>().text = "STAD, LAND";

									newObj.transform.Find("DateText").GetComponent<Text>().text = dateText;
								}
							}
						});
					}
				}
			}
		});

		loadingScreen.GetComponent<LoadingScreen>().fadeOut = true;
		initialStart = false;
	}

	void OnEnable()
	{
		if (initialStart == false)
		{
			// Delete all crushes in the content object
			foreach (Transform child in this.transform)
			{
				GameObject.Destroy(child.gameObject);
			}

			Start();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
		{
			CrushList.SetActive(false);
		}
	}

	string getDateAgo(long timestamp)
	{
		string result = "";

		DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
		date = date.AddMilliseconds(timestamp);

		TimeSpan ts = DateTime.UtcNow - date;

		if(ts.Days > 0)
		{
			if(ts.Days == 1)
			{
				result = ts.Days + " Day ago";
			}
			else
			{
				result = ts.Days + " Days ago";
			}
		}
		else if(ts.Hours > 0)
		{
			if (ts.Hours == 1)
			{
				result = ts.Hours + " Hour ago";
			}
			else
			{
				result = ts.Hours + " Hours ago";
			}
		}
		else if (ts.Minutes > 0)
		{
			if (ts.Minutes == 1)
			{
				result = ts.Minutes + " Minute ago";
			}
			else
			{
				result = ts.Minutes + " Minutes ago";
			}
		}
		else if (ts.Seconds > 0)
		{
			result = "Just now";
		}

		return result;
	}
}