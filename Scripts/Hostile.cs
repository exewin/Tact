using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostile : MonoBehaviour 
{
	
	[SerializeField] private Transform[] bodyParts;
	[SerializeField] private LayerMask layers;
	private Renderer rend;
	
	private void Start()
	{
		rend=GetComponent<Renderer>();
	}
	
	private void Update()
	{
		for(int i = 0; i<GameController.mercs.Count;i++)
		{
			if (!Physics.Linecast(transform.position, GameController.mercs[i].transform.position,layers))
			{
				Visible();
				return;
			}
		}
		Hide();
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