using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FillDropdown : MonoBehaviour
{
	List<string> list = new List<string>();

	// Use this for initialization
	void Start ()
	{
		for (int i = 18; i <= 99; i++){
			list.Add ("" + i);
		}
	}

	public void FillDropdownBox(Dropdown dropdown){
		dropdown.ClearOptions ();
		dropdown.AddOptions(list);
	}
}

