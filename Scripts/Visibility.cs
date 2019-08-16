using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
	public int id;
	[SerializeField] private LayerMask layers;
	[SerializeField] private Transform head;
	[SerializeField] public Transform[] bodyParts;
	public List<Visibility> humans = new List<Visibility>();
	
	public void SetID(int i, List<Visibility> list)
	{
		humans = list;
		id = i;
	}


	protected virtual void Update() 
	{
		if(id!=0)
		{
			for(int i = 0; i<humans.Count;i++)
			{
				Visibility human = humans[i].GetComponent<Visibility>();
				bool canSee = false;
				if(id==human.id)
					continue;
				
				for(int j = 0; j<3; j++)
				{
					if (!Physics.Linecast(head.transform.position, humans[i].bodyParts[j].transform.position,layers))
					{
						canSee = true;
					}
				}
				if(canSee)
					ActionTrue(humans[i]);
				else
					ActionFalse(humans[i]);
			}
		}
	}
	
	protected virtual void ActionTrue(Visibility human){ }
	protected virtual void ActionFalse(Visibility human){ }
}
