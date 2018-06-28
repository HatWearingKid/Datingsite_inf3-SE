using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {
    private JSONNode questions;
    Scene m_Scene;

    //zet scenes in volgorde van aantal vragen van laag naar hoog
    //kijk bij random scene of de nieuwe scene hier bij in kan
    private List<string> Scenes = new List<string> { "scene0","scene1","scene2"};

    /// <summary>
    /// to change scene give desired scene name as string
    /// </summary>
    /// <param name="sceneName">desired scene name</param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// opens a random scene
    /// </summary>
    public void RandomScene()
    {
        //get random  int
        int tempInt;
        tempInt = GetRandom();

        // as long as new scene is equeal to current scene get new int
        while(m_Scene.name == Scenes[tempInt])
        {
            tempInt = GetRandom();
        }

        //load new scene
        SceneManager.LoadScene(Scenes[tempInt]);
    }

    /// <summary>
    /// generates a random int based on amount of questions that are left for user
    /// </summary>
    /// <returns></returns>
    private int GetRandom()
    {
        int random;
        //get ammount of questions left
        questions = this.GetComponent<getQuestions>().ReturnAmmountQuestions();

        
        //if there are over to nine questions
        if (questions.Count >= 9)
        {
            //return random number
            random = Random.Range(0, Scenes.Count - 1);
            SceneManager.LoadScene(Scenes[random]);
        }
        //if there are over to seven questions 
        if (questions.Count >= 7)
        {
            //return random number
            random = Random.Range(0, 1);
            SceneManager.LoadScene(Scenes[random]);
        }
        //else load return number one
        else
        {
            //return 1
            random = 1;
        }
        return random;
    }
}
