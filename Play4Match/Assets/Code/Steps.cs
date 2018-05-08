using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steps : MonoBehaviour {

    public GameObject pawn;
    public GameObject camera;
    private int stepNumber = 0;
    public float duration = 1.0F;
    private Vector3 startPoint;
    private Vector3 startPointCamera;
    private float startTime;
    
    Vector3[] Pawnpositions = { new Vector3 { x = -571, y = 0, z = -103 },
                        new Vector3 { x = 130, y = 0, z = 174},
                        new Vector3 { x = 794, y = 0, z = 439 },
                        new Vector3 { x = 462, y = 0, z = 949 }};
    private Vector3 cameraStand;
    
    // Use this for initialization
    void Start () {
        pawn = GameObject.Find("pion");
        camera = GameObject.Find("Main Camera");
        startPointCamera = camera.transform.position;
        cameraStand.x = Pawnpositions[stepNumber].x;
        cameraStand.y = 1186;
        cameraStand.z = Pawnpositions[stepNumber].z - 1643;

    }
    
    // Update is called once per frame
    void Update () {
        pawn.transform.position = Vector3.Lerp(startPoint, Pawnpositions[stepNumber], (Time.time - startTime) / duration);
        camera.transform.position = Vector3.Lerp(startPointCamera, cameraStand, (Time.time - startTime) / duration);
    }
   
    
    
    //put pawn on next place
    public void Step ()
    {
        stepNumber += 1;
        if (stepNumber >= Pawnpositions.Length)
        {
            stepNumber = 0;
        }
        startTime = Time.time;
        startPoint = pawn.transform.position;
        startPointCamera = camera.transform.position;
        
        cameraStand.x = Pawnpositions[stepNumber].x;
        cameraStand.y = 1186;
        cameraStand.z = Pawnpositions[stepNumber].z - 1643;
        
        
        
        /*camera = -569 1186 -1540
        pion = -571 0 - 103*/
    }
}
