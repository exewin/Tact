using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Inventory
{
	
	public InventoryController inv;

	void OnMouseDown()
	{
		inv.TakeItem(items);
	}

}
