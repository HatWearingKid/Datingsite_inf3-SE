using System.Collections.Generic;
using UnityEngine;

public class Player {
	public string Name;
	public string DateOfBirth;
	public string Gender;
	public string Description;
    public int Distance;
	public bool Location;
	public bool Liked;
	public bool Preferences;
	public bool Chatrooms;

	public Player(){
		Location = false;
		Liked = false;
		Preferences = false;
		Chatrooms = false;
	}
}