using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
	private int id;
	[SerializeField] private LayerMask layers;
	
	[SerializeField] private Transform head;
	
	public void SetID(int i)
	{
		id = i;
	}


	void Update () 
	{
		for(int i = 0; i<GameController.enemies.Count;i++)
		{
			if (!Physics.Linecast(head.transform.position, GameController.enemies[i].transform.position,layers))
			{
				//GameController.enemies[i].GetComponent<Hostile>().Visible();
				if(id==GameController.mercActive)
				{
					GameController.enemies[i].GetComponent<Hostile>().ChangeColor(Color.red);
				}
			}
			else
			{
				if(id==GameController.mercActive)
				{
					GameController.enemies[i].GetComponent<Hostile>().ChangeColor(Color.black);
				}
			}
		}
	}
}
