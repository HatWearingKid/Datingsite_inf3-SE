﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using SimpleJSON;

public class GetGPSLocation : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	DatabaseReference locRef;

	Toast toast = new Toast();

	float latitude;
	float longitude;
	string city;
	string countryLongname;
	string countryShortname;
	public Text textLat;
	public Text textLong;

	WWW www;
	private JSONNode jsonNode;

	string wwwtext;

	void Start(){
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");

		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;
		locRef = FirebaseDatabase.DefaultInstance.RootReference.Child ("Users").Child (user.UserId).Child ("Location");

		StartCoroutine (GetLocation());
		www = new WWW("https://maps.googleapis.com/maps/api/geocode/json?latlng=52.777791,6.911333&key=AIzaSyAcd1isbfsa7-pRLkmM6UTqqDtNTRf-O0A&language=en");
		StartCoroutine (WaitForRequest(www));
	}

	public void GetLocationOnClick(){
		StartCoroutine (GetLocation());
	}

	public IEnumerator GetLocation(){
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;

		// Start service before querying location
		Input.location.Start();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			toast.MyShowToastMethod("Getting GPS location timed out.");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			toast.MyShowToastMethod("Unable to determine device location.");
			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			latitude = Input.location.lastData.latitude;
			longitude = Input.location.lastData.longitude;
			toast.MyShowToastMethod ("Location synced");
			//SetLocation();
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();
	}

	public void SetLocation(){
		textLat.text = this.latitude.ToString ();
		textLong.text = this.longitude.ToString ();
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		if (www.isDone)
		{
			// check for errors
			if (www.error == null)
			{
				//remove brackets and split on comma
				string temp = www.text.Trim(new System.Char[] { '[', ']' });
				wwwtext = www.text;
				//parse json to variable
				jsonNode = JSON.Parse(www.text);

				city = jsonNode [0] [0] ["address_components"] [1] ["long_name"];
				countryLongname = jsonNode [0] [0] ["address_components"] [4] ["long_name"];
				countryShortname = jsonNode [0] [0] ["address_components"] [4] ["short_name"];
			}
			else
			{
			}
		}
	}

	public void UploadGPSLocation(){
		// Update data in the Location root
		locRef.Child ("Longitude").SetValueAsync (longitude);
		locRef.Child ("Latitude").SetValueAsync (latitude);
	
		locRef.Child ("City").SetValueAsync (city);
		locRef.Child ("CountryLong").SetValueAsync (countryLongname);
		locRef.Child ("CountryShort").SetValueAsync (countryShortname);
	}
}