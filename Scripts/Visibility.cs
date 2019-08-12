using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
	private int id;
	[SerializeField]
	private LayerMask layers;
	
	public void SetID(int i)
	{
		id = i;
	}


	void Update () 
	{
		for(int i = 0; i<GameController.enemies.Count;i++)
		{
			if (Physics.Linecast(transform.position, GameController.enemies[i].transform.position, layers))
			{
				if(id==GameController.mercActive)
				{
					GameController.enemies[i].GetComponent<Renderer>().material.color = Color.black;
				}
			}
			else
			{
				GameController.enemies[i].GetComponent<Renderer>().enabled = true;
				GameController.enemies[i].layer = 9;
				if(id==GameController.mercActive)
				{
					GameController.enemies[i].GetComponent<Renderer>().material.color = Color.red;
				}
			}
		}
	}
}
