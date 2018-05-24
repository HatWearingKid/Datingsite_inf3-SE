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
    public ParticleSystem particles;

	public GameObject impact;

	bool landed = false;
	public int speed;

	SpriteRenderer sr;
	public float fadeOutTime;
	private float startingTime;

	public string buttonName;

    public string userId;
    public string nameString;
    public string matchRateString;

    // Use this for initialization
    void Start () {
		sr = impact.GetComponent<SpriteRenderer>();
		startingTime = fadeOutTime;
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

        // If not arrived keep moving
        if(!landed)
        {
            Vector3 newPos = new Vector3(transform.position.x, 5, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);

			// Stop moving
			if (transform.position == newPos)
			{
				if (particles != null)
				{
					particles.Stop();
				}

				impact.SetActive(true);

				impact.transform.localScale = Vector3.Lerp(impact.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime / 5);

				if (impact.transform.localScale.x >= 0.3 && fadeOutTime > 0)
				{
					float alpha = 1 / startingTime * fadeOutTime;

					Color tmpColor = sr.color;
					tmpColor.a = alpha;
					sr.color = tmpColor;
					
					// Decreasing time
					fadeOutTime -= Time.deltaTime;
				}

				if(impact.transform.localScale.x > 0.5 && fadeOutTime == 0)
				{
					landed = true;
				}
			}
		}
	}
}
