using System.Collections.Generic;
using UnityEngine;

public class Player {
	public string Name;
	public string PhotoUrl;
	public string DateOfBirth;
	public string Dender;
	public string Country;
}

public class Liked {
	public int ID;
}

public class LikedBy {
	public int ID;
}

public class Preferences {
	public int AgeMax;
	public int AgeMix;
	public string Gender;
}

public class Chatrooms {
	public string ID;
}