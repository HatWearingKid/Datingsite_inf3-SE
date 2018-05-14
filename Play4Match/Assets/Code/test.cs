using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class test : MonoBehaviour {

    public GameObject[] Positions;


    void Start()
    {
        //get posities and sort by name
        Positions = GameObject.FindGameObjectsWithTag("positie").OrderBy(go => go.name).ToArray();

   


        for (int i = 0; i < Positions.Length; i++)
        {
            Debug.Log("Positie" + i + " is named " + Positions[i].name);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
