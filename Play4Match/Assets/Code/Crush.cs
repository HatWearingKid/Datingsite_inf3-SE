using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour {
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference reference;

	public GameObject matchPanel;
	public GameObject matchButton;

    public void insertCrush()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

        string id = "xh4S3DibGraTqCn8HascIIvdFR02";

        // INSERT THE CRUSH INTO DB
        string crushId = matchButton.GetComponent<CreateMatchPopup>().userId;

		//string key = reference.Child("Users").Child(id).Child("Liked").Push().Key;
		//reference.Child("Users").Child(id).Child("Liked").Child(crushId).SetValueAsync(ServerValue.Timestamp);

		// Deactivate popup and remove matchbutton
		matchButton.GetComponent<CreateMatchPopup>().OnCrush();
		matchPanel.SetActive(false);
    }
}
