using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
    public SetQuestions SetQuestions = null;
    private Vector3 cameraStand;
    public SplineController splineController;
    private bool next;
    private bool StepLock;
    private bool stop;
    public GameObject NextSceneButton;

    private Touch initTouch = new Touch();
    private float movZ = 0f;
    public float moveSpeed = 50F;
    public float direction = 1f;
    private bool movecamera = false;
    public float maxCameraPlace = 2000;

    private System.DateTime timeClicked;

    // Use this for initialization
    void Start () {
        Pawnpositions.Clear();
        //get posities and sort by name
        //Positions = GameObject.FindGameObjectsWithTag("positie").OrderBy(go => go.name).ToArray();

        for (int i = 0; i < Positions.Length; i++)
        {
            Vector3 temp = new Vector3 { x = Positions[i].transform.position.x, y = Positions[i].transform.position.y, z = Positions[i].transform.position.z, };
            Pawnpositions.Add(temp);
        }
        cameraStand.z = Pawnpositions[stepNumber].z - 453;
        cameraStand.y = 1481;
        cameraStand.x = 510;
        cameraPos.transform.position = new Vector3 { x = cameraStand.x, z = cameraStand.z, y = cameraStand.y };
        startPointCamera = cameraPos.transform.position;
    }

    Ray ray;
    private string nextlocation;

    // Update is called once per frame
    void Update () {
		
        //camera alleen bewegen als de volgende stap is aangeklikt
        if (movecamera)
        {
            cameraPos.transform.position = Vector3.Lerp(startPointCamera, cameraStand, (Time.time - startTime) / duration);
            if (cameraPos.transform.position.Equals(cameraStand))
            {
                movecamera = false;
            }
        }

        //wanneer de pion bijna op positie is
        if(Vector3.Distance(Positions[stepNumber].transform.position, pawn.transform.position) <= 10)
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
            next = false;
            GameObject pion = pawn.gameObject.transform.GetChild(0).gameObject;
            //pion.transform.position = new Vector3(pawn.transform.position.x, Random.Range(0.0f, 50.0f), pawn.transform.position.z);
        }

        //wanneer volgende stap wordt geselecteerd
        if (Input.GetMouseButtonDown(0) && (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || Input.touchCount>0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
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
                        timeClicked = System.DateTime.Now;
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
                    if(movZ >= -44 && movZ <= maxCameraPlace)
                    {
                        cameraPos.transform.position = new Vector3(cameraStand.x, 1481, movZ);
                    }
                    

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
        System.DateTime timeNow = System.DateTime.Now;
        if (!QuestionLock && (stepNumber > 0) && next)
        {
            var diffInSeconds = (timeNow - timeClicked).TotalSeconds;
            //Debug.Log(diffInSeconds);
            if(diffInSeconds > 1)
            {
                //show question
                if(SetQuestions != null)
                {
                    SetQuestions.ShowQuestion(position - 1);
                }
                else
                {
                    getQuestions.ShowQuestion(position - 1);
                }
                
                QuestionLock = true;
                next = false;

                if (Pawnpositions.Count - 1 == position)
                {
                    NextSceneButton.SetActive(true);
                }
            }
            
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