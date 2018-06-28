using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;
using System.Globalization;

public class CreateCrushList : MonoBehaviour {
	public GameObject prefab;

	public GameObject CrushList;

	private bool initialStart = true;

	public GameObject loadingScreen;

    public GameObject CrushViewPanel;
    public GameObject CrushViewPanel_ProfilePicture;
    public GameObject crushViewPanel_NameAndAge;
	public GameObject crushViewPanel_Location;
	public GameObject crushViewPanel_Description;
    public GameObject crushViewPanel_UncrushButton;
    public GameObject crushViewPanel_CrushButton;

    public GameObject ProgressBar;

    int CrushItem = 0;
    Sprite sprite;
    bool Check;

    void Start()
	{
		loadingScreen.SetActive(true);

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        string userId = auth.CurrentUser.UserId;

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

                            string crushName = snapshot2.Child("Name").Value.ToString();
                            string crushAge = getAge(snapshot2.Child("DateOfBirth").Value.ToString());
                            string crushDescription = snapshot2.Child("Description").Value.ToString();
                            string crushLocation = snapshot2.Child("Location").Child("City").Value.ToString() + ", " + snapshot2.Child("Location").Child("CountryLong").Value.ToString();

                            if (crushName != "" && crushAge != "")
                            {
                                GameObject newObj = (GameObject)Instantiate(prefab, transform);
                                newObj.name = CrushItem.ToString();
                                newObj.transform.Find("NameAgeText").GetComponent<Text>().text = crushName + " (" + crushAge + ")";
                                newObj.transform.Find("LocationText").GetComponent<Text>().text = crushLocation;

                                newObj.transform.Find("DateText").GetComponent<Text>().text = dateText;
                                newObj.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { CreateView(crushName, crushAge, crushLocation, crushDescription, snapshot2.Key, newObj); });
                                    CrushItem++;
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

    void CreateView(string name, string age, string location, string description, string crushId, GameObject crushObj)
    {
        string ppUrl = "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + crushId + "%2FProfilePicture.png?alt=media";
        StartCoroutine(FinishDownload(ppUrl));

		crushViewPanel_NameAndAge.GetComponent<Text>().text = name + " (" + age + ")";
		crushViewPanel_Location.GetComponent<Text>().text = location;
		crushViewPanel_Description.GetComponent<Text>().text = description;

		crushViewPanel_UncrushButton.SetActive(true);
		crushViewPanel_CrushButton.SetActive(false);

		crushViewPanel_UncrushButton.GetComponent<Uncrush>().CrushId = crushId;
		crushViewPanel_UncrushButton.GetComponent<Uncrush>().CrushObj = crushObj;

		crushViewPanel_CrushButton.GetComponent<Uncrush>().CrushId = crushId;
		crushViewPanel_CrushButton.GetComponent<Uncrush>().CrushObj = crushObj;
        CrushViewPanel_ProfilePicture.SetActive(true);

        CrushViewPanel.SetActive(true);
    }

    IEnumerator FinishDownload(string url)
    {
        ProgressBar.SetActive(true);
        Check = true;
        WWW imageUrl = new WWW(url);
        while (!imageUrl.isDone)
        {
            yield return null;
        }

        if (!string.IsNullOrEmpty(imageUrl.error))
        {
            Debug.Log("Download failed");
        }
        else
        {
            if (Check)
            {
                Debug.Log("Download succes");
                sprite = Sprite.Create(imageUrl.texture, new Rect(0, 0, imageUrl.texture.width, imageUrl.texture.height), new Vector2(0, 0));
                yield return new WaitForSecondsRealtime(1);
                CrushViewPanel_ProfilePicture.GetComponent<Image>().sprite = sprite;
                Check = false;
            }
        }
        ProgressBar.SetActive(false);
    }

    public void ResetSprite()
    {
        CrushViewPanel_ProfilePicture.GetComponent<Image>().sprite = null;
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
		else if (ts.Seconds >= 0)
		{
			result = "Just now";
		}

		return result;
	}

    string getAge(string date)
    {

        string[] seperateNumbers = date.Split('/');

        DateTime birthdate = DateTime.Today;

        if (seperateNumbers[0].Length == 1)
        {
            seperateNumbers[0] = "0" + seperateNumbers[0];
        }

        if (seperateNumbers[1].Length == 1)
        {
            seperateNumbers[1] = "0" + seperateNumbers[1];
        }

        birthdate = DateTime.ParseExact(seperateNumbers[0]+"/"+ seperateNumbers[1]+"/"+seperateNumbers[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);

        // Save today's date.
        var today = DateTime.Today;

        if (!birthdate.Equals(today))
        {
            // Calculate the age.
            var age = today.Year - birthdate.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthdate > today.AddYears(-age)) age--;

            return age.ToString();
        }

        return "";
    }
}