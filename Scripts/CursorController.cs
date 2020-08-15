using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour 
{
	
	[SerializeField] private GameObject UIChance;
	[HideInInspector] public StatsMerc[] mercs = new StatsMerc[6];
	[SerializeField] private LayerMask mouseLayers;
	[SerializeField] private LayerMask chanceToHitLayers;
	
	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject(-1)==false) // is the touch on the GUI
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, mouseLayers))
			{
				if(hit.transform.tag=="Shootable")
				{
					if(hit.collider.GetComponent<BodyPart>().owner==mercs[GameController.mercActive])
						return;
					
					if(Input.GetMouseButtonDown(0))
					{
						mercs[GameController.mercActive].SetTarget(hit.transform);
					}
					
					if(mercs[GameController.mercActive].weapon) //UI CtH
					{
						Vector3 screenPoint = Camera.main.WorldToScreenPoint (hit.point);
						UIChance.transform.position = screenPoint;
						float CtH;
						if(Physics.Linecast(mercs[GameController.mercActive].GetComponent<Stats>().head.position, hit.transform.position,chanceToHitLayers))
						{
							CtH = 0;
						}
						else
						{
							CtH = Formulas.ChanceToHit(mercs[GameController.mercActive].GetComponent<Stats>(), hit.transform);
						}
						UIChance.GetComponent<Text>().text = hit.collider.name + "\nCtH:"+CtH.ToString("F0")+"%";
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
					mercs[GameController.mercActive].navMeshAgent.SetDestination(hit.point);
				}
			}
		}

		
	}
}
