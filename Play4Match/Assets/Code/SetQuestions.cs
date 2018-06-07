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
        Debug.Log(questionId);
        if(questionId == 0)
        {
            question1.SetActive(true);
        }
    }

}
