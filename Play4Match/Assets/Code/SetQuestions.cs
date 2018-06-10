using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetQuestions : MonoBehaviour {

    public GameObject question1;
    public GameObject question2;
    public GameObject question3;
    public GameObject question4;
    public GameObject question5;
    public GameObject question6;
    public GameObject question7;

    public void ShowQuestion(int questionId)
    {
        if(questionId == 0)
        {
            question1.SetActive(true);
        }
		else if (questionId == 1)
		{
			question2.SetActive(true);
		}
		else if (questionId == 2)
		{
			question3.SetActive(true);
		}
		else if (questionId == 3)
		{
			question4.SetActive(true);
		}
		else if (questionId == 4)
		{
			question5.SetActive(true);
		}
		else if (questionId == 5)
		{
			question6.SetActive(true);
		}
		else if (questionId == 6)
		{
			question7.SetActive(true);
		}
	}

}
