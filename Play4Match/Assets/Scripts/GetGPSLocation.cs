using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGPSLocation : MonoBehaviour {
	Toast toast = new Toast();

	float latitude;
	float longitude;
	public Text textLat;
	public Text textLong;

	public void StartGPS(){
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
			SetLocation(Input.location.lastData.latitude, Input.location.lastData.longitude);
		}

		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();
	}

	public void SetLocation(float latitude, float longitude){
		textLat.text = latitude.ToString ();
		textLong.text = longitude.ToString ();
	}
}