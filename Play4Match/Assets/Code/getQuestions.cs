using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
/using SimpleJSON;

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

    // Use this for initialization
    void Start () {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";//auth.CurrentUser.UserId; //forceert nu eelco's account anders moet ik elke keer inloggen om te testen

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

            Debug.Log(JasonData);
            for (int key = 0; key< JasonData.Count; key++)
            {
                Debug.Log(JasonData[key]["Q"].Value);
            }  
            
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

            Debug.Log("there are no more questions to be asked");
            //put in pop-up
            TempCanvas = NoQuestion.transform.Find("Question");
            questiontext = TempCanvas.GetComponent<Text>();
            NoQuestion.SetActive(true);
            questiontext.text = "there are no more questions to be asked";
        }
        else
        {
            //selecteerd op basis van antwoordnummers welk popup moet komen.
            if (JasonData[QuestionNumber]["Answers"].Count == 3)
            {
                //vind de text waar vraag ingezet moet worden
                TempCanvas = vragenQuestion3.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];

                //importeerd antwoorden vanuit jsondata naar de antwoord slider



                //zet de correcte canvas in een variabele zodat deze later gebruikt kan worden om de canvas te hiden
                AantalAntwoorden = vragenQuestion3;
                //activeer canvas
                vragenQuestion3.SetActive(true);


                

            }
            if (JasonData[QuestionNumber]["Answers"].Count == 4)
            {
                TempCanvas = vragenQuestion4.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];
                AantalAntwoorden = vragenQuestion4;
                vragenQuestion4.SetActive(true);
            }
            if (JasonData[QuestionNumber]["Answers"].Count == 5)
            {
                TempCanvas = vragenQuestion5.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];
                AantalAntwoorden = vragenQuestion5;
                vragenQuestion5.SetActive(true);
            }
            if (JasonData[QuestionNumber]["Answers"].Count == 6)
            {
                TempCanvas = vragenQuestion6.transform.Find("Question");
                TempCanvas.GetComponent<Text>().text = JasonData[QuestionNumber]["Q"];

                AantalAntwoorden = vragenQuestion6;
                vragenQuestion6.SetActive(true);
            }
            currentQuestionNumber = QuestionNumber;


        }
        
    }
    public void ChangeAnswer(int answer)
    {
        answerNumber = answer;
    }

    public void ChangeWeight(int Weight)
    {
        weightNumber = Weight;
    }

    public void SendAnswer()
    {
        //send currentQuestionNumber, answerNumber and weightNumber
        Debug.Log("ik verzend nu vraagnummer: " + currentQuestionNumber + " met antwoordnummer: " + answerNumber + " met het gewicht van: " + weightNumber);
        //sendToOtherScript();
        AantalAntwoorden.SetActive(false);
    }
}
