using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCanvas : MonoBehaviour {
    public Canvas OptionsCanvas;
    private void Start()
    {
        OptionsCanvas= GameObject.Find("OptionsCanvas").GetComponent<Canvas>();
        OptionsCanvas.GetComponent<Canvas>().enabled = false;
    }
    
    public Canvas CanvasObject;
    /// <summary>
    /// to change canvas give desired canvas name as string
    /// </summary>
    /// <param name="canvasName">desired canvas name</param>
    public void ActivateCanvas(string canvasName)
    {
        CanvasObject = GameObject.Find(canvasName).GetComponent<Canvas>();
        OptionsCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void DeactivateCanvas(string canvasName)
    {
        CanvasObject = CanvasObject = GameObject.Find(canvasName).GetComponent<Canvas>();
        CanvasObject.GetComponent<Canvas>().enabled = false;
    }

}
