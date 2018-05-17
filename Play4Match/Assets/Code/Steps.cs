using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Steps : MonoBehaviour {

    public GameObject pawn;
    //public GameObject camera;
    private int stepNumber = 0;
    public float duration = 1.0F;
    private Vector3 startPoint;
    private Vector3 startPointCamera;
    private float startTime;
    public GameObject[] Positions;
    public List<Vector3> Pawnpositions;
    private bool QuestionLock;
    public getQuestions getQuestions;

    //Vector3[] Pawnpositions;
    /*= { new Vector3 { x = -571, y = 0, z = -103 },
                    new Vector3 { x = 130, y = 0, z = 174},
                    new Vector3 { x = 794, y = 0, z = 439 },
                    new Vector3 { x = 462, y = 0, z = 949 }};*/
    private Vector3 cameraStand;
    
    // Use this for initialization
    void Start () {
        //get posities and sort by name
       // pawn = GameObject.Find("pion");
        Positions = GameObject.FindGameObjectsWithTag("positie").OrderBy(go => go.name).ToArray();

        for (int i = 0; i < Positions.Length; i++)
        {
            Vector3 temp = new Vector3 { x = Positions[i].transform.position.x, y = Positions[i].transform.position.y, z = Positions[i].transform.position.z, };

            //Debug.Log(temp);
            Pawnpositions.Add(temp);
            //Debug.Log("Positie" + i + " is named " + Positions[i].name);
        }



        
       // camera = GameObject.Find("Main Camera");
       // startPointCamera = camera.transform.position;
        //cameraStand.x = Pawnpositions[stepNumber].x;
        //cameraStand.y = 1186;
       // cameraStand.z = Pawnpositions[stepNumber].z - 1643;

    }

    Ray ray;
    private string nextlocation;
    // Update is called once per frame
    void Update () {
        pawn.transform.position = Vector3.Lerp(startPoint, Pawnpositions[stepNumber], (Time.time - startTime) / duration);
        //camera.transform.position = Vector3.Lerp(startPointCamera, cameraStand, (Time.time - startTime) / duration);

        //SHOULD detect if stuff is touched
        /*if(Input.touchCount > 0 )
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Debug.DrawRay(ray.origin, ray.direction = Input.GetTouch(0).position);
            if (Physics.Raycast(ray, Mathf.Infinity))
            {
                Debug.Log("hit something");
            }
        }*/
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform objecthit = hit.transform;
                if (hit.transform.gameObject.tag == "positie")
                {
                    nextlocation = hit.collider.gameObject.ToString();
                    //Debug.Log(hit.collider.gameObject.ToString());
                    //Debug.Log("i touched somethign");
                    if(Positions[stepNumber + 1].ToString().Equals(nextlocation))
                    {
                        Step();
                    }
                    
                }
            }
        }

        if (pawn.transform.position.x.Equals(Positions[stepNumber].transform.position.x) && pawn.transform.position.z.Equals(Positions[stepNumber].transform.position.z) && !QuestionLock)
            {
            
                Debug.Log("question time");
                //show question
                getQuestions.ShowQuestion(stepNumber);
                QuestionLock = true;
            }





    }
   
    
    
    //put pawn on next place
    public void Step ()
    {
        stepNumber += 1;
        if (stepNumber >= Pawnpositions.Count)
        {
            stepNumber = 0;
        }
        startTime = Time.time;
        startPoint = pawn.transform.position;
        QuestionLock = false;

        // startPointCamera = camera.transform.position;

        //  cameraStand.x = Pawnpositions[stepNumber].x;
        // cameraStand.y = 1186;
        // cameraStand.z = Pawnpositions[stepNumber].z - 1643;



        /*camera = -569 1186 -1540
        pion = -571 0 - 103*/
    }

    public void ChangeSpeed(float newSpeed)
    {
        duration = newSpeed;
    }
}