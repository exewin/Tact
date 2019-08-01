using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour 
{
	
	public InventoryController inv; // TEMP TODO

	void OnMouseDown()
	{
		Debug.Log("sd");
		inv.TakeItem(GetComponent<Inventory>().items);
	}

}
