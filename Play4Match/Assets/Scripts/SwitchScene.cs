using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {
    public getQuestions getquestions;
    private JSONNode questions;
    Scene m_Scene;

    //zet scenes in volgorde van aantal vragen van laag naar hoog
    //kijk bij random scene of de nieuwe scene hier bij in kan
    private List<string> Scenes = new List<string> { "scene0","scene1"};

    /// <summary>
    /// to change scene give desired scene name as string
    /// </summary>
    /// <param name="sceneName">desired scene name</param>
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RandomScene()
    {
        int tempInt;
        tempInt = getRandom();

        while(m_Scene.name == Scenes[tempInt])
        {
            tempInt = getRandom();
        }
        SceneManager.LoadScene(Scenes[tempInt]);
    }

    private int getRandom()
    {
        int random;
        questions = getquestions.ReturnAmmountQuestions();
        Debug.Log(questions.Count);

        if (questions.Count >= 7)
        {
            random = Random.Range(0, 1);
            SceneManager.LoadScene(Scenes[random]);
        }
        if (questions.Count >= 9)
        {
            random = Random.Range(0, Scenes.Count - 1);
            SceneManager.LoadScene(Scenes[random]);
        }
        else
        {
            random = 1;
        }
        return random;
    }
}
