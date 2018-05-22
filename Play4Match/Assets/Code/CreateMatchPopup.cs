using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMatchPopup : MonoBehaviour {
    public GameObject matchPanel;
    private Image image;

    public GameObject nameObj;
    public GameObject ageObj;
    public GameObject genderObj;
    public GameObject matchRateObj;
    public GameObject closeButtonObj;

    public string objName = "MatchButton";

    public string nameString;
    public string ageString;
    public string genderString;
    public string matchRateString;

    // Use this for initialization
    void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Debug.Log(nameString);

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform objecthit = hit.transform;

                if (matchPanel.active == false && hit.transform.gameObject.tag == objName)
                {
                        nameObj.GetComponent<Text>().text = nameString;
                        ageObj.GetComponent<Text>().text = ageString;
                        genderObj.GetComponent<Text>().text = genderString;
                        matchRateObj.GetComponent<Text>().text = matchRateString;

                        matchPanel.SetActive(true);
                }
            }
        }
    }
}
