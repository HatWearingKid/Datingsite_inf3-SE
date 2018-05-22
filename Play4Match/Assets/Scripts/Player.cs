using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string DateOfBirth;
	public string Dender;
	public string Country;
}

public class Liked : MonoBehaviour {
	public int ID;
}

public class LikedBy : MonoBehaviour {
	public int ID;
}

public class Preferences : MonoBehaviour {
	public int AgeMax;
	public int AgeMix;
	public string Gender;
}

public class Chatrooms : MonoBehaviour {
	public string ID;
}