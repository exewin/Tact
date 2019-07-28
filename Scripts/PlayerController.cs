using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public int id;
	public Camera cam;
	public NavMeshAgent agent;
	
	void Update()
	{
		if(id==GlobalScript.mercActive)
		{
			if(Input.GetMouseButtonDown(1))
			{
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit))
				{
					agent.SetDestination(hit.point);
				}
			}
			GetComponent<Renderer>().material.color = Color.yellow;
		}
		else
			GetComponent<Renderer>().material.color = Color.white;
	}
	
	
	void OnMouseDown()
	{
		//GlobalScript.SelectMerc(id); TO DO
	}
}