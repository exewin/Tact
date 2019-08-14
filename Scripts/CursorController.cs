using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour 
{
	
	[SerializeField]
	private GameObject UIChance;
	[HideInInspector]
	public StatsMerc[] mercs = new StatsMerc[6];
	
	[SerializeField]
	private LayerMask layers;
	
	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject(-1)==false) // is the touch on the GUI
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit)) ///LAYERY KURWA TODO
			{
				if(hit.transform.tag=="Shootable")
				{
					if(hit.collider.GetComponent<BodyPart>().owner==mercs[GameController.mercActive])
						return;
					
					if(Input.GetMouseButtonDown(0))
					{
						mercs[GameController.mercActive].Shoot(hit.transform);
					}
					
					if(mercs[GameController.mercActive].weapon) //UI CtH
					{
						Vector3 screenPoint = Camera.main.WorldToScreenPoint (hit.point);
						UIChance.transform.position = screenPoint;
						int CtH;
						if(Physics.Linecast(mercs[GameController.mercActive].GetComponent<Stats>().head.position, hit.transform.position,layers))
						{
							CtH = 0;
						}
						else
						{
							CtH = (int)Formulas.ChanceToHit(Formulas.Distance(mercs[GameController.mercActive].GetComponent<Stats>().head, hit.transform),
							mercs[GameController.mercActive].accuracy,mercs[GameController.mercActive].weapon.accuracy);
						}
						UIChance.GetComponent<Text>().text = hit.collider.name + "\nCtH:"+CtH+"%";
					}
				}
				else
				{
					UIChance.transform.position = new Vector3(-100,0,0);
				}			
			}
		

			if(Input.GetMouseButtonDown(1))
			{
				if(Physics.Raycast(ray, out hit))
				{
					mercs[GameController.mercActive].GetComponent<NavMeshAgent>().SetDestination(hit.point);
				}
			}
		}

		
	}
}
