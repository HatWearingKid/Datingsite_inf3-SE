using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public GameObject soundEngine;
	
	public Slider pawnSpeed;
	public Toggle soundToggle;
	public Slider volume;
	
	public SplineController splineController;

	
	// Use this for initialization
	void Start () {
		soundToggle.onValueChanged.AddListener(delegate {
                SoundToggleChanged();
				});
		volume.onValueChanged.AddListener(delegate {
                SoundVolumeChanged();
				});
		pawnSpeed.onValueChanged.AddListener(delegate {
                PawnSpeedChanged();
				});
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void SoundToggleChanged()
	{
		soundEngine.GetComponent<SoundScript>().PlaySound("toggleSound");
		soundEngine.GetComponent<SoundScript>().SetMute();
	}
	
	void SoundVolumeChanged()
	{
		float vol = volume.value;
		soundEngine.GetComponent<SoundScript>().SetVolume(vol);
	}
	
	void PawnSpeedChanged()
	{
		splineController.mSplineInterp.updateList(pawnSpeed.value);
	}
}
