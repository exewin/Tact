using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{

	public List<Item> items = new List<Item>();
	public GameObject slotPrefab;
	public Transform gridParent;
	List<GameObject> backpackSlots = new List<GameObject>();
	
	
	
	void Start()
	{
		UpdateInventory();
	}
	
	public void UpdateInventory()
	{
		ClearSlots(); //delete existing slots
		CreateSlots(); //create new slots
	}
	
	void ClearSlots()
	{
		for(int i = 0; i<backpackSlots.Count;i++)
		{
			Destroy(backpackSlots[i]);
		}
	}
	
	void CreateSlots()
	{
		for(int i = 0; i<items.Count;i++)
		{
			GameObject slot = Instantiate(slotPrefab);
			slot.transform.SetParent(gridParent,false);
			//slot.transform.localScale = new Vector3(1,1,1); //use false instead ^
			slot.GetComponent<UIBackpackSlot>().Assign(items[i]);
			backpackSlots.Add(slot);
		}
	}
	
}
