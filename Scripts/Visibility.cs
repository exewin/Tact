using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
	int id;
	List<GameObject> enemies = new List<GameObject>();
	public LayerMask layers;
	void Start()
	{
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("Hostile"))
		{
			enemies.Add(e);
		}
	}

	void Update () 
	{
		for(int i = 0; i<enemies.Count;i++)
		{
			if (Physics.Linecast(transform.position, enemies[i].transform.position, layers))
			{
				if(id==GameController.mercActive)
				{
					enemies[i].GetComponent<Renderer>().material.color = Color.black;
				}
			}
			else
			{
				enemies[i].GetComponent<Renderer>().enabled = true;
				if(id==GameController.mercActive)
				{
					enemies[i].GetComponent<Renderer>().material.color = Color.red;
					
				}
			}
		}
	}
	
	public void SetID(int i)
	{
		id = i;
	}
}
