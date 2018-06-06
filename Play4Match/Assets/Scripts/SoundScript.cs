using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {

	private AudioSource source;
	
	public AudioClip sliderSound;
	public AudioClip muteSound;
	public AudioClip matchSound;
	
	public bool mute;
	
	// Use this for initialization
	void Start () {
		mute = false;
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			source.Play();
		}
	}
	
	public void SetMute()
	{
		mute = !mute;
		source.mute = mute;
	}
	
	public void SetVolume(float volume)
	{
		source.volume = volume;
	}
		
	
	public void PlaySound(string soundName)
	{
		switch(soundName)
		{
			case "sliderSound":
				source.clip = sliderSound;
				break;
			case "muteSound":
				source.clip = muteSound;
				break;
			case "matchSound":
				source.clip = matchSound;
				break;
			default: 
				source.clip = sliderSound;
				break;
		}
		
	}
}

