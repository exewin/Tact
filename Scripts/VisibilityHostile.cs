using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityHostile : Visibility
{

	private Renderer rend;
	float hideTimer = 0;
	
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
	
	public void ChangeColor(Color c)
	{
		rend.material.color = c;
	}

}