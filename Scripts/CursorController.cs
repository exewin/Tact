using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour 
{
	
	public Texture2D cursorTexture;
    private Vector2 hotSpot = Vector2.zero;
	
	public StatsMerc[] mercs = new StatsMerc[6];
	
	void Start()
	{
		hotSpot = new Vector2 (cursorTexture.width / 2, cursorTexture.height / 2);
	}
	
	void Update()
	{
		if (EventSystem.current.IsPointerOverGameObject(-1)==false)    // is the touch on the GUI
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.transform.tag=="Shootable")
				{
					if(Input.GetMouseButtonDown(0))
					{
						mercs[GameController.mercActive].Shoot(hit.transform);
					}
					
					Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
				}		
			}
		

			if(Input.GetMouseButtonDown(1))
			{
				if(Physics.Raycast(ray, out hit))
				{
					mercs[GameController.mercActive].GetComponent<NavMeshAgent>().SetDestination(hit.point);
				}
			}
					//GetComponent<Renderer>().material.color = Color.yellow;			
					//GetComponent<Renderer>().material.color = Color.white;
		}
		
	}
}
