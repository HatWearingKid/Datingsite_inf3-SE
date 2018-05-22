using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {

	Toast toast = new Toast();
	private GameObject mainPanel;
	private GameObject registerPanel;
	private GameObject loginPanel;
	private GameObject resetPassPanel;
	private GameObject instructionsPanel;

	void Start () {
		mainPanel = GameObject.Find("MainPanel");
		registerPanel = GameObject.Find("RegisterPanel");
		loginPanel = GameObject.Find("LoginPanel");
		resetPassPanel = GameObject.Find("ResetPwPannel");
		instructionsPanel = GameObject.Find("Instructions-panel");
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{ 
			toast.MyShowToastMethod ("Back to main menu");
			mainPanel.SetActive(true);
			registerPanel.SetActive(false);
			loginPanel.SetActive(false);
			resetPassPanel.SetActive(false);
			instructionsPanel.SetActive(false);
		}
	}

}
