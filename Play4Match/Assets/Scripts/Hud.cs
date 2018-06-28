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
		
		// Add button click
		menuButton.onClick.AddListener(ButtonClicked);
		
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		string userId = auth.CurrentUser.UserId;
		
		FirebaseDatabase.DefaultInstance.GetReference("Users").Child(userId).GetValueAsync().ContinueWith(
		task => {
			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;
                nameText.text = snapshot.Child("Name").Value.ToString();
			}
		});
	}
	
	void ButtonClicked()
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
