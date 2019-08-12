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
		gameObject.layer = 10;
	}
	
	
	void Saw(bool mode)
	{
		rend.material.color = Color.black;
	}

}