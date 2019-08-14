using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Inventory
{
	
	private InventoryController inv;
	
	void Start()
	{
		inv = Camera.main.GetComponent<InventoryController>();
	}
	
	void OnMouseDown()
	{
		inv.TakeItem(items);
		Destroy(gameObject);
	}

}
