using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile : MonoBehaviour 
{
	
	public Transform[] bodyParts;
	
	public bool sawByActive;
	public Renderer rend;
	
	void Start()
	{
		rend=GetComponent<Renderer>();
	}

	//?
	void FixedUpdate()
	{
		rend.enabled = false;
	}
	
	
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			for(int i=0;i<3;i++)
				if(hit.transform==bodyParts[i])
					if(Input.GetMouseButtonDown(0))
						Debug.Log("Enemy was shoot in the "+hit.transform.name);
		}
		else
		{
			
		}
	}
	
	void Saw(bool mode)
	{
		rend.material.color = Color.black;
	}
	
	
	void OnMouseOver()
	{
		//Debug.Log("Enemy hovered");
	}

}