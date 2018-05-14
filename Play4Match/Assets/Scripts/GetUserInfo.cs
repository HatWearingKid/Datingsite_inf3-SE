using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUserInfo : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

	public void GetData(){
		Debug.Log (auth.CurrentUser.UserId);
	}
}