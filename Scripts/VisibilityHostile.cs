﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityHostile : Visibility
{

	private Renderer rend;
	private float hideTimer = 0;
	
	private void Start()
	{
		rend=GetComponent<Renderer>();
		Hide();
	}
	
	protected override void Update()
	{
		base.Update();
		if(hideTimer>0)
		{
			hideTimer -= Time.deltaTime*1;
		
			if(hideTimer<=0)
				Hide();
		}
	}
	
	protected override void ActionTrue(Visibility human)
	{
		if(human.tag == "Player")
		{
			if(!GetComponent<StatsHostile>().currentTarget)
			GetComponent<StatsHostile>().SetTarget(human.bodyParts[1]); // chest
		}
	}		
	
	protected override void ActionFalse(Visibility human)
	{
		if(human.tag == "Player")
		{
			if(GetComponent<StatsHostile>().currentTarget) //popraw te referencje
				GetComponent<StatsHostile>().SetTarget(null);
		}
	}	
	
	
	private void Hide()
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
		hideTimer = 1;
		rend.enabled = true;
		gameObject.layer = 9;
		for(int i = 0; i < bodyParts.Length; i++)
		{
			bodyParts[i].gameObject.layer = 9;
		}
	}


}