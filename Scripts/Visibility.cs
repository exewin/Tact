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

	public void BodyPartsResize(state mode)
	{
		if(mode == state.stand)
		{
			bodyParts[0].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.2f, .5f);
			bodyParts[0].transform.localPosition = new Vector3(0, 1.25f, 0);
			bodyParts[1].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.5f, .5f);
			bodyParts[1].transform.localPosition = new Vector3(0, 0.55f, 0);
			bodyParts[2].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.3f, .5f);
			bodyParts[2].transform.localPosition = new Vector3(0, -0.25f, 0);
		}
		else if(mode == state.crouch)
		{
			bodyParts[0].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.2f, .5f);
			bodyParts[0].transform.localPosition = new Vector3(0, 0.7f, 0);
			bodyParts[1].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.2f, .5f);
			bodyParts[1].transform.localPosition = new Vector3(0, 0.3f, 0);
			bodyParts[2].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.3f, .5f);
			bodyParts[2].transform.localPosition = new Vector3(0, -0.2f, 0);
		}
		else if(mode == state.crawl)
		{
			bodyParts[0].GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
			bodyParts[0].transform.localPosition = new Vector3(0, -0.4f, 0);
			bodyParts[1].GetComponent<BoxCollider>().size = new Vector3(.5f, 0.2f, 1);
			bodyParts[1].transform.localPosition = new Vector3(0, -0.4f, 0);
			bodyParts[2].GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
			bodyParts[2].transform.localPosition = new Vector3(0, -0.4f, 0);
		}
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
