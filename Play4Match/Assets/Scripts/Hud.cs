using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Hud : MonoBehaviour {
	
	WWW www;
    private JSONNode JsonData;
	
	public GameObject menuPanel;
	public Button menuButton;
	public Text nameText;

	// Use this for initialization
	void Start () {
		menuButton.GetComponent<Button>();
		menuButton.onClick.AddListener(buttonClicked);
		
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		//string userId = "xh4S3DibGraTqCn8HascIIvdFR02";
		string userId = auth.CurrentUser.UserId;
        Debug.Log(userId);
		FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userId).GetValueAsync().ContinueWith(
		task => {
			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;

                nameText.text = snapshot.Child("Name").Value.ToString();
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
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
