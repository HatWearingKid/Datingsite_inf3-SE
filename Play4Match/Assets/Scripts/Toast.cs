using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toast. Class used to show toast messages on Android devices
/// </summary>
public class Toast {
	string toastString;
	AndroidJavaObject currentActivity;

	public void MyShowToastMethod (string message)
	{
		// Check if the running device is an Android device
		if (Application.platform == RuntimePlatform.Android) {
			showToastOnUiThread (message);
		}
	}

	void showToastOnUiThread(string toastString){
		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

		currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		this.toastString = toastString;

		currentActivity.Call ("runOnUiThread", new AndroidJavaRunnable (showToast));
	}

	void showToast(){
		Debug.Log ("Running on UI thread");
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
		AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
		AndroidJavaObject javaString=new AndroidJavaObject("java.lang.String",toastString);
		AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject> ("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
		toast.Call ("show");
	}

}