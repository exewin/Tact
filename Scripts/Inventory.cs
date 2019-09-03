using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{

	[SerializeField] private List<Item> startItems = new List<Item>();
	 public List<Item> items = new List<Item>();
	
	private void Awake()
	{
		for(int i = 0; i < startItems.Count; i++)
		{
			if(startItems[i])
			{
				AddItem(startItems[i]);
			}
		}
	}
	
	public void AddItem(Item item)
	{
		if(item.stackable)
		{
			if(FindItemInInventory(item.name))
			{
				for(int j = 0; j < items.Count; j++)
				{
					if(items[j].name == item.name)
					{
						items[j].quantity+=item.quantity;
						break;
					}
				}
			}
			else
			{
				Item j = Instantiate(item);
				items.Add(j);
			}
		}
		else
		{
			Item j = Instantiate(item);
			items.Add(j);
		}
	}
	
	public void RemoveItem(Item item)
	{
		items.Remove(item);
	}
	
		#region find
	private bool FindItemInInventory(string s)
	{
		for(int i = 0; i < items.Count; i++)
		{
			if(items[i].name == s)
			{
				return true;
			}
		}
		return false;
	}
	#endregion	
}
