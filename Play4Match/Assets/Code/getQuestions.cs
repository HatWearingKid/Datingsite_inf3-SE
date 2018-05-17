using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class getQuestions : MonoBehaviour {

    string[] questionArray;
    WWW www;
    private int currentQuestionNumber = 0;
    private int answerNumber = 1;
    private int weightNumber = 1;
    // Use this for initialization
    void Start () {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = "xh4S3DibGraTqCn8HascIIvdFR02";//auth.CurrentUser.UserId; //forceert nu eelco's account anders moet ik elke keer inloggen om te testen

        string url = "http://play4match.com/api/getq.php?id=" + userid;
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
            string temp = www.text.Trim(new System.Char[] {'[', ']'});
            questionArray = temp.Split(',');

/*
            foreach (var vraag in questionArray)
            {
                Debug.Log(vraag.Trim(new System.Char[] {'"' }));
            }*/
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    // Update is called once per frame
    void Update () {

	}

    public Text questiontext;
    
    public void ShowQuestion(int QuestionNumber)
    {
        //make question apear on screen
        //questiontext = GameObject.Find("Question").GetComponent<Text>();
        if (QuestionNumber >= questionArray.Length)
        {
            Debug.Log("there are no more questions to be asked");
            //put in pop-up
            questiontext.text = "there are no more questions to be asked";
        }
        else
        {
            currentQuestionNumber = QuestionNumber;
            Debug.Log("question number: " + QuestionNumber);
            Debug.Log(questionArray[QuestionNumber].Trim(new System.Char[] { '"' }));
            //put in pop-up
            questiontext.text = questionArray[QuestionNumber].Trim(new System.Char[] { '"' });

            
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
    }
}
