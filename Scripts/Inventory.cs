using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{

	public List<Item> startItems = new List<Item>();
	[HideInInspector]
	public List<Item> items = new List<Item>();
	
	private void Awake()
	{
		for(int i = 0; i < startItems.Count; i++)
		{
			if(startItems[i])
			{
				Item j = Instantiate(startItems[i]);
				items.Add(j);
			}
		}
	}
}
