using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;

public class User : MonoBehaviour {
	private string userID;
	private string name;
	private string gender;
	private string genderPreference;
	private string country;
	private string dateOfBirth;

	public User(string userID){
		this.userID = userID;
	}

	/*public User(string name, string gender, string genderPreference, string country, string dateOfBirth){
		this.name = name;
		this.gender = gender;
		this.genderPreference = genderPreference;
		this.country = country;
		this.dateOfBirth = dateOfBirth;
	}*/
}