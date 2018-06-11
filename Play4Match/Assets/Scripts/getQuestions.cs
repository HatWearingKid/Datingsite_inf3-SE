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

public class getQuestions : MonoBehaviour
{
	public GameObject loadingScreen;

    string[] questionArray;
    WWW www;
    private int currentQuestionId = 0;
    private int answerNumber = 1;
    private int weightNumber = 1;
    public int NumberOfQuestions;
    private JSONNode JsonData;

    public GameObject QuestionPanel;
    public GameObject QuestionText;
	public GameObject Answers2;
	public GameObject Answers3;
    public GameObject Answers4;
    public GameObject Answers5;
    public GameObject Answers6;
    public GameObject weightSlider;

    public GameObject NoQuestion;

    private Text questiontext;
    private GameObject AantalAntwoorden;
    private string userid;
    private DatabaseReference reference;

    // Use this for initialization
    void Start()
    {
        //connect to firebase and get userid
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        userid = "xh4S3DibGraTqCn8HascIIvdFR02";

        //set reference to firebase database
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://play4matc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //get json from api
        string url = "http://play4match.com/api/getq.php?id=" + userid + "&qamount=" + NumberOfQuestions;

		loadingScreen.SetActive(true);

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

				loadingScreen.GetComponent<LoadingScreen>().fadeOut = true;
			}
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }



    public void ShowQuestion(int questionId)
    {
		// Deactive all Answer sliders
		Answers2.SetActive(false);
		Answers3.SetActive(false);
        Answers4.SetActive(false);
        Answers5.SetActive(false);
        Answers6.SetActive(false);

        // If there are no questions
        if (questionId >= JsonData.Count)
        {
            //Show Popup
            NoQuestion.SetActive(true);
        }
        else
        {
            // Set QuestionText
            QuestionText.GetComponent<Text>().text = JsonData[questionId]["Q"];

			if (JsonData[questionId]["Answers"].Count == 2)
			{
				// Fill all the answers
				for (int i = 0; i < 2; i++)
				{
					Answers2.transform.Find("Text" + (i + 1)).GetComponent<Text>().text = UppercaseFirst(JsonData[questionId]["Answers"][i]);
				}

				// Activate panel
				Answers2.SetActive(true);
			}
			else if (JsonData[questionId]["Answers"].Count == 3)
            {
                // Fill all the answers
                for(int i = 0; i < 3; i++)
                {
                    Answers3.transform.Find("Text"+(i+1)).GetComponent<Text>().text = UppercaseFirst(JsonData[questionId]["Answers"][i]);
                }

                // Activate panel
                Answers3.SetActive(true);
            }
            else if(JsonData[questionId]["Answers"].Count == 4)
            {
                // Fill all the answers
                for (int i = 0; i < 4; i++)
                {
                    Answers4.transform.Find("Text" + (i + 1)).GetComponent<Text>().text = UppercaseFirst(JsonData[questionId]["Answers"][i]);
                }

                // Activate panel
                Answers4.SetActive(true);
            }
            else if (JsonData[questionId]["Answers"].Count == 5)
            {
                // Fill all the answers
                for (int i = 0; i < 5; i++)
                {
                    Answers5.transform.Find("Text" + (i + 1)).GetComponent<Text>().text = UppercaseFirst(JsonData[questionId]["Answers"][i]);
                }

                // Activate panel
                Answers5.SetActive(true);
            }
            else if (JsonData[questionId]["Answers"].Count == 6)
            {
                // Fill all the answers
                for (int i = 0; i < 6; i++)
                {
                    Answers6.transform.Find("Text" + (i + 1)).GetComponent<Text>().text = UppercaseFirst(JsonData[questionId]["Answers"][i]);
                }

                // Activate panel
                Answers6.SetActive(true);
            }

            // Activate the QuestionPanel Popup
            QuestionPanel.SetActive(true);

            // Save currenQuestionId
            currentQuestionId = JsonData[questionId]["Id"];
        }
    }

    //send answer to firebase
    public void SendAnswer()
    {
        int answer = 0;
        int weight = (int)weightSlider.GetComponent<Slider>().value;

        if(Answers3.activeSelf == true)
        {
            answer = (int)Answers3.GetComponent<Slider>().value;
        }
        else if (Answers4.activeSelf == true)
        {
            answer = (int)Answers4.GetComponent<Slider>().value;
        }
        else if (Answers5.activeSelf == true)
        {
            answer = (int)Answers5.GetComponent<Slider>().value;
        }
        else if (Answers6.activeSelf == true)
        {
            answer = (int)Answers6.GetComponent<Slider>().value;
        }

        //change answer object to json string
        string sendAnswer = "{\"answer\":" + answer + ", \"weight\":" + weight + "}";

        //send json string to firebase database
        reference.Child("Users").Child(userid).Child("Answered").Child(currentQuestionId.ToString()).SetRawJsonValueAsync(sendAnswer);

		// Deactive QuestionPanel and reset weightslider
		weightSlider.GetComponent<Slider>().value = 1;
        QuestionPanel.SetActive(false);
    }

	string UppercaseFirst(string s)
	{
		// Check for empty string.
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		// Return char and concat substring.
		return char.ToUpper(s[0]) + s.Substring(1);
	}
}