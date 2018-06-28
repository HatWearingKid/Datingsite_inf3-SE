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
        //clear pawnpostions , otherwise it copy's from other scene
        Pawnpositions.Clear();

        for (int i = 0; i < Positions.Length; i++)
        {
            //add positions based on 3d models
            Vector3 temp = new Vector3 { x = Positions[i].transform.position.x, y = Positions[i].transform.position.y, z = Positions[i].transform.position.z, };
            Pawnpositions.Add(temp);
        }
        //set camera position
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
		
        //camera only moves if nextstep is clicked
        if (movecamera)
        {
            //move camera
            cameraPos.transform.position = Vector3.Lerp(startPointCamera, cameraStand, (Time.time - startTime) / duration);
            //if current position equeals current position stop movement
            if (cameraPos.transform.position.Equals(cameraStand))
            {
                movecamera = false;
            }
        }

        //when pawn is almost on position
        if(Vector3.Distance(Positions[stepNumber].transform.position, pawn.transform.position) <= 17)
        {
            //stop movement
            splineController.mSplineInterp.mState = "Stopped";
            stop = false;
            if (stepNumber > 0)
            {
                StepLock = false;
                next = true;
            }
        } else
        {
            //else follow curve
            next = false;
            GameObject pion = pawn.gameObject.transform.GetChild(0).gameObject;
            
        }

        //when next step is selected
        if (Input.GetMouseButtonDown(0) && (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || Input.touchCount>0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
            //if raycast hits next step
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
                        //go to the next step
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
                if (!movecamera && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || Input.touchCount > 0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    //move camera according to finger movement
                    float deltaZ = initTouch.position.y - touch.position.y;
                    movZ = deltaZ * Time.deltaTime * moveSpeed * direction;
                    movZ = movZ + cameraPos.transform.position.z;
                    if(movZ >= -44 && movZ <= maxCameraPlace )
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

                //if it is not the first time playing
                if(SetQuestions != null)
                {
                    SetQuestions.ShowQuestion(position - 1);
                }
                //if it is the first time playing
                else
                {
                    getQuestions.ShowQuestion(position - 1);
                }
                
                QuestionLock = true;
                next = false;

                //if pawn is on last position show the next scene button
                if (Pawnpositions.Count - 3 == position)
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
            //camera to the next position
            cameraStand.z = Pawnpositions[position].z - 553;
            position += 1;
        }
        //set mState to let pawn follow line
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