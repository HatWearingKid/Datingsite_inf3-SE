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
    public SplineController splineController;
    private bool next;
    private bool StepLock;
    private bool stop;

    private Touch initTouch = new Touch();
    private float movZ = 0f;
    public float moveSpeed = 50F;
    public float direction = 1f;
    private bool movecamera = false;

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
        if (movecamera)
        {
            cameraPos.transform.position = Vector3.Lerp(startPointCamera, cameraStand, (Time.time - startTime) / duration);
            if (cameraPos.transform.position.Equals(cameraStand))
            {
                movecamera = false;
            }
        }

        //wanneer de pion bijna op positie is
        if(Vector3.Distance(Positions[stepNumber].transform.position, pawn.transform.position) <= 7)
        {
            //stop beweging
            splineController.mSplineInterp.mState = "Stopped";
            stop = false;
            if (stepNumber > 0)
            {
                StepLock = false;
                next = true;
            }
        } else
        {
            //anders follow curve
            pawn.transform.position = new Vector3(pawn.transform.position.x, Random.Range(0.0f, 50.0f), pawn.transform.position.z);
        }

        //wanneer volgende stap wordt geselecteerd
        if (Input.GetMouseButtonDown(0)&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Transform objecthit = hit.transform;
                if (hit.transform.gameObject.tag == "positie")
                {
                    nextlocation = hit.collider.gameObject.ToString();
                    if (!StepLock && stepNumber + 1 < Positions.Length && Positions[stepNumber + 1].ToString().Equals(nextlocation))
                    {
                        //zet stap
                        StepLock = true;
                        Step();                          
                    }
                }
                
            }
        }
        getQuestion();

        //see if screen is touched
        foreach(Touch touch in Input.touches)
        {
            //see if touch just began
            if (touch.phase == TouchPhase.Began)
            {
                initTouch = touch;
            }
            //if the touch is moving
            else if(touch.phase == TouchPhase.Moved)
            {
                //if there is no popup over the level or if the camera is not locked
                if (!movecamera && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    //move camera according to finger movement
                    float deltaZ = initTouch.position.y - touch.position.y;
                    movZ = deltaZ * Time.deltaTime * moveSpeed * direction;
                    movZ = movZ + cameraPos.transform.position.z;
                    cameraPos.transform.position = new Vector3(608, 1481, movZ);
                }
            }
            //if touch ends , reset variables
            else if (touch.phase == TouchPhase.Ended)
            {
                initTouch = new Touch();
                movZ = 0;
            }
        }

    }
    private int position = 1;
    //put pawn on next place

    public void getQuestion()
    {
        if (!QuestionLock && (stepNumber > 0) && next)
        {
            //show question
            getQuestions.ShowQuestion(position);
            QuestionLock = true;
            next = false;
        }
    }
    public void Step ()
    {

        movecamera = true;
        QuestionLock = false;
        stepNumber += 1;
        if (stepNumber > 1)
        {
            //camera naar volgende positie
            cameraStand.z = Pawnpositions[position].z - 553;
            position += 1;
        }
        //set mState zodat pion lijn weer volgt
        splineController.mSplineInterp.mState = "";
        
        if (stepNumber >= Pawnpositions.Count)
        {
            stepNumber = 0;
        }

        startTime = Time.time;
        startPoint = pawn.transform.position;

        startPointCamera = cameraPos.transform.position;
        
    }

    public void ChangeSpeed(float newSpeed)
    {
        duration = newSpeed;
    }
}