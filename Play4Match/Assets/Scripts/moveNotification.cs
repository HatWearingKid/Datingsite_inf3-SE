using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveNotification : MonoBehaviour {

	public int moveDownSpeed;
	bool deleteThis;

	Vector3 target;
	Vector3 deleteTarget;
	
	
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Button>().onClick.AddListener( () =>
		{
			DeleteThis();
		});
		
		transform.position = new Vector3(transform.position.x, 0 + Screen.height, transform.position.z);
		
		
		target = new Vector3(transform.position.x, Screen.height - 100, transform.position.z);
		//target = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		
		deleteTarget = new Vector3(transform.position.x, (transform.position.y + 200), transform.position.z);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!deleteThis)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, moveDownSpeed * Time.deltaTime);
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, deleteTarget, moveDownSpeed * Time.deltaTime);
			Destroy(this.gameObject, 2);
		}
	}
	
	void DeleteThis()
	{
		deleteThis = true;
	}
}
