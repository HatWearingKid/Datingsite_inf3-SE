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
    public Dropdown dropdownMin;
    public Dropdown dropdownMax;

	// Use this for initialization
	void Start ()
	{
        dropdownMin.ClearOptions();
        dropdownMax.ClearOptions();
        for (int i = 18; i <= 99; i++){
			list.Add ("" + i);
		}
        dropdownMin.AddOptions(list);
        dropdownMax.AddOptions(list);
    }
}

