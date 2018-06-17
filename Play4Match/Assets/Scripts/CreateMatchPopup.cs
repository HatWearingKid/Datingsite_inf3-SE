using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMatchPopup : MonoBehaviour {
    public GameObject matchPanel;

    public Image image;
    public GameObject nameObj;
    public GameObject locationObj;
    public GameObject matchRateObj;
	public GameObject descriptionObj;
	public GameObject closeButtonObj;
    public GameObject crushButtonObj;
    public ParticleSystem particles;

	public GameObject impact;

	public GameObject RedHeart;
	public GameObject WhiteHeart;

	bool landed = false;
	public int speed;
	float stopCounter = 0;

	SpriteRenderer sr;
	public float fadeOutTime;
	private float startingTime;

	public string buttonName;

    Sprite sprite;
    public string userId;
    public string nameString;
    public string locationString;
    public string matchRateString;
	public string descriptionString;

	// Use this for initialization
	void Start () {
		sr = impact.GetComponent<SpriteRenderer>();
		startingTime = fadeOutTime;

        string ppUrl = "https://firebasestorage.googleapis.com/v0/b/play4matc.appspot.com/o/ProfilePictures%2F" + userId + "%2FProfilePicture.png?alt=media";
        StartCoroutine(FinishDownload(ppUrl));
    }

    IEnumerator FinishDownload(string url)
    {
        WWW imageUrl = new WWW(url);

        while (!imageUrl.isDone)
        {
            yield return null;
        }

        if (!string.IsNullOrEmpty(imageUrl.error))
        {
            Debug.Log("Download failed");
        }
        else
        {
            yield return new WaitForSecondsRealtime(1);
            Debug.Log("Download succes");
            sprite = Sprite.Create(imageUrl.texture, new Rect(0, 0, imageUrl.texture.width, imageUrl.texture.height), new Vector2(0, 0));
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform objecthit = hit.transform;

                if (matchPanel.active == false && hit.transform.gameObject.name == buttonName)
                {
                    nameObj.GetComponent<Text>().text = nameString;
                    locationObj.GetComponent<Text>().text = locationString;
                    descriptionObj.GetComponent<Text>().text = descriptionString;
                    matchRateObj.GetComponent<Text>().text = matchRateString;
					descriptionObj.GetComponent<Text>().text = descriptionString;
					crushButtonObj.GetComponent<Crush>().matchButton = GameObject.Find(buttonName);
                    image.GetComponent<Image>().sprite = sprite;

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
			if(transform.position == newPos)
			{
				stopCounter += Time.deltaTime * 5;

				if (particles != null && stopCounter >= 1)
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

				if (impact.transform.localScale.x > 0.5 && fadeOutTime == 0)
				{
					landed = true;
				}
			}			
		}
	}

	public void OnCrush()
	{
		float currentX = transform.position.x;
		float currentY = transform.position.y;
		float currentZ = transform.position.z;
	
		int maxHearts = Random.Range(10, 20);

		// Destroy matchbutton after spawning some lovely hearts
		for (int i = 0; i < maxHearts; i++)
		{
			GameObject heart = Instantiate(RedHeart);

			if (Random.Range(0, 2) == 1)
			{
				heart = Instantiate(WhiteHeart);
			}

			// Set new X and Z values from the heart
			float newX = Random.Range(currentX - 125f, currentX + 125f);
			float newY = Random.Range(currentY, currentY + 125f);
			float newZ = Random.Range(currentZ - 100f, currentZ + 100f);

			int heartSize = Random.Range(50,200);

			heart.transform.position = new Vector3(newX, newY, newZ);
			heart.transform.localScale = new Vector3(heartSize, heartSize, 1);

			heart.SetActive(true);
		}

		Destroy(this.gameObject);
		
	}
}
