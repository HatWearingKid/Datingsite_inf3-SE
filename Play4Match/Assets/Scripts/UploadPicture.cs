using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Storage;

// Profiel foto ophalen uit de storage
// Storage link voorbeeld: storageref/profilepictures/userid/picture.png
// Map aanmaken bij het registreren van gebruiker

public class UploadPicture : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	FirebaseStorage storage;
	StorageReference profilePictureRef;
	string imagePath = "";

	void Start(){
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		user = auth.CurrentUser;

		// Get a reference to the storage service, using the default Firebase App
		storage = FirebaseStorage.DefaultInstance;

		// Create a storage reference from our storage service
		profilePictureRef = storage.GetReferenceFromUrl("gs://play4matc.appspot.com/ProfilePictures");
	}

	public void PickImageFromGallery( int maxSize = 1024 )
	{
		NativeGallery.GetImageFromGallery( ( path ) =>
			{
				if( path != null )
				{
					imagePath = path;
				}
			}, maxSize: maxSize );

		Toast toast = new Toast ();
		toast.MyShowToastMethod (imagePath);
	}

	public void Upload(){
		// Upload the file to the path "images/rivers.jpg"
		profilePictureRef.PutFileAsync(imagePath)
			.ContinueWith ((Task<StorageMetadata> task) => {
				if (task.IsFaulted || task.IsCanceled) {
					Debug.Log(task.Exception.ToString());
					// Uh-oh, an error occurred!
				} else {
					// Metadata contains file metadata such as size, content-type, and download URL.
					Firebase.Storage.StorageMetadata metadata = task.Result;
					string download_url = metadata.DownloadUrl.ToString();
					Debug.Log("Finished uploading...");
					Debug.Log("download url = " + download_url);
				}
			});
	}
}