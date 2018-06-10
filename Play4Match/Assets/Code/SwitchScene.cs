using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

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
        int random = Random.Range(0, Scenes.Count);
        SceneManager.LoadScene(Scenes[random]);
    }
}
