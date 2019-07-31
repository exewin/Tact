using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{

	public int id;
	public List<GameObject> enemies = new List<GameObject>();
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
				if(id==GlobalScript.mercActive)
				{
					enemies[i].GetComponent<Renderer>().material.color = Color.black;
					
				}
			}
			else
			{
				enemies[i].GetComponent<Renderer>().enabled = true;
				if(id==GlobalScript.mercActive)
				{
					enemies[i].GetComponent<Renderer>().material.color = Color.red;
					
				}
			}
		}
	}
}
