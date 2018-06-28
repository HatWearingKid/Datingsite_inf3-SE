using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {

	private AudioSource source;
	
	public AudioClip sliderSound;
	public AudioClip toggleSound;
	public AudioClip matchSound;
	public AudioClip notificationSound;
	
	
	// Use this for initialization
	void Start () 
	{
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		// Deze input kan weg, anders krijg je elke klik een sound
		if(Input.GetMouseButtonDown(0))
		{
			//source.Play();
		}
	}
	
	// Reverse mute state
	public void SetMute()
	{
		source.mute = !source.mute;
	}
	
	// Set volume
	public void SetVolume(float volume)
	{
		source.volume = volume;
	}
		
	// Play sound depending on string
	public void PlaySound(string soundName)
	{
		switch(soundName)
		{
			case "sliderSound":
				source.clip = sliderSound;
				source.Play();
				break;
			case "toggleSound":
				source.clip = toggleSound;
				source.Play();
				break;
			case "matchSound":
				source.clip = matchSound;
				source.Play();
				break;
			case "notificationSound":
				source.clip = notificationSound;
				source.Play();
				break;
			default: 
				source.clip = toggleSound;
				source.Play();
				break;
		}
		
	}
}

