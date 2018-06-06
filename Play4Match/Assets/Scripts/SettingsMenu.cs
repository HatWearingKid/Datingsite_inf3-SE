using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public GameObject soundEngine;
	
	public Toggle soundToggle;
	public Slider volume;

	
	// Use this for initialization
	void Start () {
		soundToggle.onValueChanged.AddListener(delegate {
                SoundToggleChanged();
				});
		volume.onValueChanged.AddListener(delegate {
                SoundVolumeChanged();
				});
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void SoundToggleChanged()
	{
		soundEngine.GetComponent<SoundScript>().SetMute();
	}
	
	void SoundVolumeChanged()
	{
		float vol = volume.value;
		soundEngine.GetComponent<SoundScript>().SetVolume(vol);
	}
}
