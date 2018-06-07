using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveHeart : MonoBehaviour {

	SpriteRenderer sr;

	Vector3 startPosition;
	Vector3 endPosition;

	public float fadeOutTime_min;
	public float fadeOutTime_max;
	private float fadeOutTime;
	public int speed;

	private float startingTime;

    Vector3 newPos;

    // Use this for initialization
    void Start () {
		sr = this.GetComponent<SpriteRenderer>();
		startPosition = transform.position;

		fadeOutTime = Random.Range(fadeOutTime_min, fadeOutTime_max);
		startingTime = fadeOutTime;

        newPos = new Vector3(transform.position.x, startPosition.y + 1000f, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
		
		transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);


		if (fadeOutTime > 0)
		{
			float alpha = 1 / startingTime * fadeOutTime;

			Color tmpColor = sr.color;
			tmpColor.a = alpha;
			sr.color = tmpColor;

			// Decreasing time
			fadeOutTime -= Time.deltaTime;
		}
		
		if(fadeOutTime < 0)
		{
			Destroy(this.gameObject);
		}
		
	}
}
