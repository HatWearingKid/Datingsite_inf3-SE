using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Steps : MonoBehaviour {

    public GameObject pawn;
    public GameObject cameraPos;
    private int stepNumber = 0;
    public float duration = 1.0F;
    private Vector3 startPoint;
    private Vector3 startPointCamera;
    private float startTime;
    public GameObject[] Positions;
    public List<Vector3> Pawnpositions;
    private bool QuestionLock;
    public getQuestions getQuestions;
    private Vector3 cameraStand;
    
    // Use this for initialization
    void Start () {
        //get posities and sort by name
        Positions = GameObject.FindGameObjectsWithTag("positie").OrderBy(go => go.name).ToArray();

        for (int i = 0; i < Positions.Length; i++)
        {
            Vector3 temp = new Vector3 { x = Positions[i].transform.position.x, y = Positions[i].transform.position.y, z = Positions[i].transform.position.z, };
            Pawnpositions.Add(temp);
        }
        cameraStand.z = Pawnpositions[stepNumber].z - 553;
        cameraStand.y = 1481;
        cameraStand.x = 608;
        cameraPos.transform.position = new Vector3 { x = cameraStand.x, z = cameraStand.z, y = cameraStand.y };
        startPointCamera = cameraPos.transform.position;
    }

    Ray ray;
    private string nextlocation;
    // Update is called once per frame
    void Update () {
        pawn.transform.position = Vector3.Lerp(startPoint, Pawnpositions[stepNumber], (Time.time - startTime) / duration);
        cameraPos.transform.position = Vector3.Lerp(startPointCamera, cameraStand, (Time.time - startTime) / duration);


        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform objecthit = hit.transform;
                if (hit.transform.gameObject.tag == "positie")
                {
                    nextlocation = hit.collider.gameObject.ToString();
  
                    if(stepNumber+1 <Positions.Length && Positions[stepNumber + 1].ToString().Equals(nextlocation))
                    {
                        Step();
                    }
                    
                }
            }
        }

        if (pawn.transform.position.x.Equals(Positions[stepNumber].transform.position.x) && pawn.transform.position.z.Equals(Positions[stepNumber].transform.position.z) && !QuestionLock)
            {

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

        startPointCamera = cameraPos.transform.position;
        cameraStand.z = Pawnpositions[stepNumber].z - 553;
    }

    public void ChangeSpeed(float newSpeed)
    {
        duration = newSpeed;
    }
}