using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMatchPopup : MonoBehaviour {
    public GameObject matchPanel;
    private Image image;

    public GameObject nameObj;
    public GameObject matchRateObj;
    public GameObject closeButtonObj;
    public GameObject crushButtonObj;

    public int speed;

    public string buttonName;

    public string userId;
    public string nameString;
    public string matchRateString;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform objecthit = hit.transform;

                if (matchPanel.active == false && hit.transform.gameObject.name == buttonName)
                {
                    nameObj.GetComponent<Text>().text = nameString;
                    matchRateObj.GetComponent<Text>().text = matchRateString;
                    crushButtonObj.GetComponent<Crush>().matchButton = GameObject.Find(buttonName);

                    matchPanel.SetActive(true);
                }
            }
        }

        Vector3 newPos = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
    }
}
