using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile : MonoBehaviour 
{
	
	public Transform[] bodyParts;
	private  Renderer rend;
	
	void Start()
	{
		rend=GetComponent<Renderer>();
	}

	//?
	void FixedUpdate()
	{
		rend.enabled = false;
		gameObject.layer = 10;
		for(int i = 0; i < bodyParts.Length; i++)
		{
			bodyParts[i].gameObject.layer = 10;
		}
	}
	
	public void Visible()
	{
		rend.enabled = true;
		gameObject.layer = 9;
		for(int i = 0; i < bodyParts.Length; i++)
		{
			bodyParts[i].gameObject.layer = 9;
		}
	}
	
	public void ChangeColor(Color c)
	{
		rend.material.color = c;
	}

}