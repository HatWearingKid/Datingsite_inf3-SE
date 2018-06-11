using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	private float startingTime;
	public float fadeOutTime;
	private float fadeOutTimeSaved;

	public GameObject heart;
	public GameObject Background;

	public bool fadeOut = false;

	// Use this for initialization
	void Start() {
		fadeOutTimeSaved = fadeOutTime;

		startingTime = fadeOutTime;
	}

	// Update is called once per frame
	void Update() {
		if(fadeOut == true)
		{
			FadeOut();
		}
	}

	void FadeOut () {
		if (fadeOutTime > 0)
		{
			float alpha = 1 / startingTime * fadeOutTime;

			Color heartColor = heart.GetComponent<Image>().color;
			heartColor.a = alpha;
			heart.GetComponent<Image>().color = heartColor;

			Color backgroundColor = Background.GetComponent<Image>().color;
			backgroundColor.a = alpha;
			Background.GetComponent<Image>().color = backgroundColor;

			// Decreasing time
			fadeOutTime -= Time.deltaTime;
		}

		if (fadeOutTime < 0)
		{
			fadeOutTime = fadeOutTimeSaved;

			Color heartColor = heart.GetComponent<Image>().color;
			heartColor.a = 1;
			heart.GetComponent<Image>().color = heartColor;

			Color backgroundColor = Background.GetComponent<Image>().color;
			backgroundColor.a = 1;
			Background.GetComponent<Image>().color = backgroundColor;

			fadeOut = false;
			this.gameObject.SetActive(false);
		}
	}
}
