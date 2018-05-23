using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {
	public GameObject main;
	public GameObject register;
	public GameObject login;
	public GameObject resetPwd;
	public GameObject instructions;

	void Start () {
		///
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape) ||	Input.GetMouseButton(1))
		{
			if(register.active)
			{
				register.SetActive(false);
				main.SetActive(true);
			}
			else if(login.active)
			{
				login.SetActive(false);
				main.SetActive(true);
			}
			else if(resetPwd.active)
			{
				resetPwd.SetActive(false);
				main.SetActive(true);
			}
			else if(instructions.active)
			{
				instructions.SetActive(false);
				main.SetActive(true);
			}
		}		
	}
}
