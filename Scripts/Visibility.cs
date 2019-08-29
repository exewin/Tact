using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
	public int id;
	[SerializeField] private LayerMask layers;
	[SerializeField] private Transform head;
	public Transform[] bodyParts;
	
	public void SetID(int i)
	{
		id = i;
	}


	protected virtual void Update() 
	{
		if(id!=0)
		{
			for(int i = 0; i<GameController.humans.Count;i++)
			{
				Visibility human = GameController.humans[i].GetComponent<Visibility>();
				bool canSee = false;
				if(id==human.id)
					continue;
				
				for(int j = 0; j<3; j++)
				{
					if (!Physics.Linecast(head.transform.position, GameController.humans[i].bodyParts[j].transform.position,layers))
					{
						canSee = true;
					}
				}
				if(canSee)
					ActionTrue(GameController.humans[i]);
				else
					ActionFalse(GameController.humans[i]);
			}
		}
	}
	
	protected virtual void ActionTrue(Visibility human){ }
	protected virtual void ActionFalse(Visibility human){ }
}
