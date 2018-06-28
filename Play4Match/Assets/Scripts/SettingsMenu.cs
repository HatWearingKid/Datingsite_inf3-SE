using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public GameObject soundEngine;
	
	public Toggle soundToggle;
	public Slider volume;

	// Use this for initialization
	void Start () 
	{
		soundToggle.onValueChanged.AddListener(delegate {
                SoundToggleChanged();
				});
		volume.onValueChanged.AddListener(delegate {
                SoundVolumeChanged();
				});
	}
	
	// Play sound and then mute
	void SoundToggleChanged()
	{
		soundEngine.GetComponent<SoundScript>().PlaySound("toggleSound");
		soundEngine.GetComponent<SoundScript>().SetMute();
	}
	
	// Get slider value and set volume
	void SoundVolumeChanged()
	{
		float vol = volume.value;
		soundEngine.GetComponent<SoundScript>().SetVolume(vol);
	}
}
