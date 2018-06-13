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

		target = new Vector3(0, 0, 0);

		deleteTarget = new Vector3(transform.position.x, (transform.position.y + 100), transform.position.z);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!deleteThis)
		{
			if(transform.GetComponent<RectTransform>().anchoredPosition.y > -42f)
			{
				transform.position = Vector3.MoveTowards(transform.position, target, moveDownSpeed * Time.deltaTime);
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, deleteTarget, moveDownSpeed * Time.deltaTime);

			if(transform.position.Equals(deleteTarget))
			{
				Destroy(this.gameObject);
			}
		}
	}
	
	void DeleteThis()
	{
		deleteThis = true;
	}
}
