using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string playerName;

	public string SaveToString()
	{
		return JsonUtility.ToJson(this);
	}
}