using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEditor;

public class getQuestions : MonoBehaviour {

    string[] questionArray;
    WWW www;
    private int currentQuestionNumber = 0;
    private int answerNumber = 1;
    private int weightNumber = 1;
    public int NumberOfQuestions;
    private JSONNode JsonData;
    public GameObject vragenQuestion3;
    public GameObject vragenQuestion4;
    public GameObject vragenQuestion5;
    public GameObject vragenQuestion6;
    public GameObject NoQuestion;
    private Text questiontext;
    private GameObject AantalAntwoorden;
    private string userid;
    private DatabaseReference reference;

    // Use this for initialization
    void Start () {
        //connect to firebase and get userid
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        userid = "xh4S3DibGraTqCn8HascIIvdFR02";//auth.CurrentUser.UserId; //forceert nu eelco's account anders moet ik elke keer inloggen om te testen

        //set reference to firebase database
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
        //get json from api
        string url = "http://play4match.com/api/getq.php?id=" + userid+ "&qamount=" + NumberOfQuestions;
        Debug.Log(url);
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.isDone)
        {
            // check for errors
            if (www.error == null)
            {
                //remove brackets and split on comma
                string temp = www.text.Trim(new System.Char[] { '[', ']' });
                questionArray = temp.Split(',');

                //parse json to variable
                JsonData = JSON.Parse(www.text);

            }
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }
        }
        
    }


    // Update is called once per frame
    void Update () {

	}


    
    public void ShowQuestion(int QuestionNumber)
    {
        //make question apear on screen
        //questiontext = GameObject.Find("Question").GetComponent<Text>();
        Transform TempCanvas;
        if (QuestionNumber >= JsonData.Count)
        {

            //Debug.Log("there are no more questions to be asked");
            //put in pop-up
            TempCanvas = NoQuestion.transform.Find("Question");
            questiontext = TempCanvas.GetComponent<Text>();
            NoQuestion.SetActive(true);
            questiontext.text = "there are no more questions to be asked";
        }
        else
        {
            //temporary gameobject
            GameObject temp;
            
            //selecteerd op basis van antwoordnummers welk popup moet komen. dit is voor alle antwoord aantallen hetzelfde
            if (JsonData[QuestionNumber]["Answers"].Count == 3)
            {
                //vind de text waar vraag ingezet moet worden
                TempCanvas = vragenQuestion3.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JsonData[QuestionNumber]["Q"];

                //importeerd antwoorden vanuit jsondata naar de antwoord slider
                temp = vragenQuestion3.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion3.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion3.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][2];

                //zet de correcte canvas in een variabele zodat deze later gebruikt kan worden om de canvas te hiden
                AantalAntwoorden = vragenQuestion3;
                //activeer canvas
                vragenQuestion3.SetActive(true);


                

            }
            if (JsonData[QuestionNumber]["Answers"].Count == 4)
            {
                TempCanvas = vragenQuestion4.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JsonData[QuestionNumber]["Q"];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][2];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text4").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][3];

                AantalAntwoorden = vragenQuestion4;
                vragenQuestion4.SetActive(true);
            }
            if (JsonData[QuestionNumber]["Answers"].Count == 5)
            {
                TempCanvas = vragenQuestion5.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JsonData[QuestionNumber]["Q"];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][2];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text4").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][3];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text5").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][4];

                AantalAntwoorden = vragenQuestion5;
                vragenQuestion5.SetActive(true);
            }
            if (JsonData[QuestionNumber]["Answers"].Count == 6)
            {
                TempCanvas = vragenQuestion6.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JsonData[QuestionNumber]["Q"];
                
                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][2];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text4").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][3];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text5").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][4];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text6").gameObject;
                temp.GetComponent<Text>().text = JsonData[QuestionNumber]["Answers"][5];

                
                AantalAntwoorden = vragenQuestion6;
                vragenQuestion6.SetActive(true);
            }
            currentQuestionNumber = JsonData[QuestionNumber]["Id"];


        }
        
    }

    //change answernumber. is used for the slider
    public void ChangeAnswer(float answer)
    {
        answerNumber = (int)answer;

    }

    //change weightNumber. is used for the slider
    public void ChangeWeight(float Weight)
    {
        weightNumber = (int)Weight;

    }

    //send answer to firebase
    public void SendAnswer()
    { 
        /*send to firebase*/
        //make answer object
        Answers answer = new Answers(answerNumber, weightNumber);
        //change answer object to json string
        string sendAnswer = JsonUtility.ToJson(answer);
        //send json string to firebase database
        reference.Child("Users").Child(userid).Child("Answered").Child(currentQuestionNumber.ToString()).SetRawJsonValueAsync(sendAnswer);

        //deactivate de panel waar antwoorden moeten worden gegeven
        AantalAntwoorden.SetActive(false);
    }
}

public class Answers {


    public int Answer;
    public int Value;

    /// <summary>
    /// answer object for sending to firebase database
    /// </summary>
    /// <param name="answer">give answer int</param>
    /// <param name="weight">the weight of the answer</param>
    public Answers(int answer, int weight)
    {
        this.Answer = answer;
        this.Value = weight;

    }

}