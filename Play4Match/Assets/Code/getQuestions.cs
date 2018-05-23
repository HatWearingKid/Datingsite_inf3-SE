using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using Firebase;
using Firebase.Database;

public class getQuestions : MonoBehaviour {

    string[] questionArray;
    WWW www;
    private int currentQuestionNumber = 0;
    private int answerNumber = 1;
    private int weightNumber = 1;
    public int NumberOfQuestions;
    private JSONNode JasonData;
    public GameObject vragenQuestion3;
    public GameObject vragenQuestion4;
    public GameObject vragenQuestion5;
    public GameObject vragenQuestion6;
    public GameObject NoQuestion;
    private Text questiontext;
    private GameObject AantalAntwoorden;
    private string userid;

    // Use this for initialization
    void Start () {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        userid = auth.CurrentUser.UserId; //"xh4S3DibGraTqCn8HascIIvdFR02";

        string url = "http://play4match.com/api/getq.php?id=" + userid+ "&qamount=" + NumberOfQuestions;
        Debug.Log(url);
        www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {

            Debug.Log("WWW Ok!: " + www.text);
            //remove brackets and split on comma
            string temp = www.text.Trim(new System.Char[] {'[', ']'});
            questionArray = temp.Split(',');

            //parse json to variable
            JasonData = JSON.Parse(www.text);

            //Debug.Log(JasonData); 
            
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
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
        if (QuestionNumber >= JasonData.Count)
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
            GameObject temp;
            
            //selecteerd op basis van antwoordnummers welk popup moet komen.
            if (JasonData[QuestionNumber]["Answers"].Count == 3)
            {
                //vind de text waar vraag ingezet moet worden
                TempCanvas = vragenQuestion3.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];

                //importeerd antwoorden vanuit jsondata naar de antwoord slider

                temp = vragenQuestion3.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion3.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion3.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][2];

                //zet de correcte canvas in een variabele zodat deze later gebruikt kan worden om de canvas te hiden
                AantalAntwoorden = vragenQuestion3;
                //activeer canvas
                vragenQuestion3.SetActive(true);


                

            }
            if (JasonData[QuestionNumber]["Answers"].Count == 4)
            {
                TempCanvas = vragenQuestion4.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][2];

                temp = vragenQuestion4.transform.Find("Antwoord").Find("Text4").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][3];

                AantalAntwoorden = vragenQuestion4;
                vragenQuestion4.SetActive(true);
            }
            if (JasonData[QuestionNumber]["Answers"].Count == 5)
            {
                TempCanvas = vragenQuestion5.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][2];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text4").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][3];

                temp = vragenQuestion5.transform.Find("Antwoord").Find("Text5").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][4];

                AantalAntwoorden = vragenQuestion5;
                vragenQuestion5.SetActive(true);
            }
            if (JasonData[QuestionNumber]["Answers"].Count == 6)
            {
                TempCanvas = vragenQuestion6.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];
                
                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text1").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][0];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text2").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][1];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text3").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][2];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text4").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][3];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text5").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][4];

                temp = vragenQuestion6.transform.Find("Antwoord").Find("Text6").gameObject;
                temp.GetComponent<Text>().text = JasonData[QuestionNumber]["Answers"][5];

                
                AantalAntwoorden = vragenQuestion6;
                vragenQuestion6.SetActive(true);
            }
            currentQuestionNumber = JasonData[QuestionNumber]["Id"];


        }
        
    }
    public void ChangeAnswer(float answer)
    {
        answerNumber = (int)answer;
        Debug.Log(answerNumber);

    }

    public void ChangeWeight(float Weight)
    {
        weightNumber = (int)Weight;

    }
    private DatabaseReference _DB;
    public void SendAnswer()
    {
        //send currentQuestionNumber, answerNumber and weightNumber
        Debug.Log(userid+" ik verzend nu vraagnummer: " + currentQuestionNumber + " met antwoordnummer: " + answerNumber + " met het gewicht van: " + weightNumber);
       
        //send to firebase
        _DB = FirebaseDatabase.DefaultInstance.GetReferenceFromUrl("https://play4matc.firebaseio.com/");
        Answers answer = new Answers(currentQuestionNumber, answerNumber, weightNumber);
        string json = JsonUtility.ToJson(answer);
        _DB.Child("Users").Child(userid).SetRawJsonValueAsync(json);


        //deactivate de panel waar antwoorden moeten worden gegeven
        AantalAntwoorden.SetActive(false);
    }
}

public class Answers {
    int id;
    int answer;
    int weight;

    public Answers(int id, int answer, int weight)
    {
        this.id = id;
        this.answer = answer;
        this.weight = weight;

    }

}