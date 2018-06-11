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
		
		var renderer = this.gameObject.GetComponentInChildren<Renderer>();
		var heightOfObject = renderer.bounds.size.y;
		
		var heightOfObject2 = 100;
		
		//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		transform.position = new Vector3(0, Screen.height + heightOfObject2, 0);
		
		
		target = new Vector3(transform.position.x, 0 + Screen.height - heightOfObject2, transform.position.z);
		//target = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		
		deleteTarget = new Vector3(transform.position.x, (transform.position.y + heightOfObject2), transform.position.z);
		
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
