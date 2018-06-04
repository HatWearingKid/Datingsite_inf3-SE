using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBeat : MonoBehaviour {

	public Sprite[] frames;
	public float framesPerSecond;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// get index of frame
		int index = (int)(Time.time * framesPerSecond) % frames.Length;

		// check if the Sprite array don't equal null
		if (frames[index] != null)
		{
			// Set sprite in gameObject
			this.gameObject.GetComponent<Image>().sprite = frames[index];
		}
	}
}
