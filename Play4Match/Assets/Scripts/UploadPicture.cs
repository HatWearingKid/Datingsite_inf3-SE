﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Storage;

public class UploadPicture : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	FirebaseStorage storage;
	StorageReference storageRef, profilePictureRef;
	string imagePath;
	Toast toast = new Toast();
	WWW www;
	public Button button;
	public Image ProfilePicture;

	void Start(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

		if (user != null)
		{
			// Get a reference to the storage service, using the default Firebase App
			storage = FirebaseStorage.DefaultInstance;

			// Create a storage reference from our storage service
			storageRef = storage.GetReferenceFromUrl("gs://play4matc.appspot.com/");
			profilePictureRef = storageRef.Child("ProfilePictures/" + user.UserId + "/ProfilePicture.png");

			RetrievePicture();
		}

	}

	public void RetrievePicture(){
		if(profilePictureRef != null)
		{
			// Fetch the download URL
			profilePictureRef.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) => {
				if (task.IsFaulted || task.IsCanceled)
				{
					toast.MyShowToastMethod("Something wrent wrong with retrieving your profile picture.");
				}
				else
				{
					if (task.Result != null)
					{
						www = new WWW(task.Result.ToString());
						StartCoroutine(GetImage(www));
					}
				}
			});
		}
	}

	public void SetProfilePicture(){
		if (www != null) {
			ProfilePicture.sprite = Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0, 0));
		}
	}

	public void SetImage(){
		if(www != null)
		{
			button.image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		}
	}

	public IEnumerator GetImage(WWW www){
		yield return www;
	}

	// Method to open the gallery on Android (with the asset)
	public void PickImageFromGallery( int maxSize = 1024 )
	{
		NativeGallery.GetImageFromGallery( ( path ) =>
			{
				if( path != null )
				{	
					imagePath = path;

					var bytes = System.IO.File.ReadAllBytes(imagePath);
					var tex = new Texture2D(1, 1);
					tex.LoadImage(bytes);

					button.image.sprite = Sprite.Create(tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0, 0));
				}
			}, maxSize: maxSize );
	}

	// Firebase method to upload files to Firebase Cloud Storage
	public void Upload(){
		if (imagePath != null) {
			// Upload the file to the path "images/rivers.jpg"
			profilePictureRef.PutFileAsync("file://" + imagePath).ContinueWith ((Task<StorageMetadata> task) => {
				if (task.IsFaulted || task.IsCanceled) {
					toast.MyShowToastMethod("Something wrent wrong, try again.");
				} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					toast.MyShowToastMethod("Upload success!");
					imagePath = null;
				}
			});
		}
	}
}