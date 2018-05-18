using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string displayName;
	public string dateOfBirth;
	public string gender;
}

public class Liked : MonoBehaviour {
	public int ID;
}

public class LikedBy : MonoBehaviour {
	public int ID;
}

public class Preferences : MonoBehaviour {
	public int ageMax;
	public int ageMix;
	public string gender;
}

public class Chatrooms : MonoBehaviour {
	public string ID;
}