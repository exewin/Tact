using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour 
{

	public List<Item> items = new List<Item>();
	public GameObject slotPrefab;
	public Transform gridParent;
	List<GameObject> backpackSlots = new List<GameObject>();
	
	
	//UI
	public Text item_name;
	public Image item_icon;
	public Text item_desc;
	public Text item_stat1;
	public Text item_stat2;
	public Text item_stat3;
	public Text item_stat4;
	
	public Text total_weigth;
	

	public void GetInventory(List<Item> mercItems)
	{
		items = mercItems; //reference instead of creating new list
		UpdateInventory();
	}	
	
	private void UpdateInventory()
	{
		ClearSlots(); //delete existing slots
		CreateSlots(); //create new slots
		CalculateWeigth(); //calculate weigth
	}
	
	private void ClearSlots()
	{
		for(int i = 0; i<backpackSlots.Count;i++)
		{
			Destroy(backpackSlots[i]);
		}
	}
	
	private void CreateSlots()
	{
		for(int i = 0; i<items.Count;i++)
		{
			GameObject slot = Instantiate(slotPrefab);
			slot.transform.SetParent(gridParent,false); //false sets localScale to (1,1,1)
			slot.GetComponent<UIBackpackSlot>().Assign(items[i]);
			backpackSlots.Add(slot);
		}
	}
	
	public void DropItem()
	{
		//TODO
		UpdateInventory();
	}	
	
	public void TakeItem(List<Item> takenItems)
	{
		//TODO
		for(int i = 0; i < takenItems.Count; i++)
		{
			if(takenItems[i].stackable)
			{
				if(items.Contains(takenItems[i]))
				{
					Item stack = items.Find(x => x.name == takenItems[i].name);
					stack.quantity+=takenItems[i].quantity;
				}
				else
					items.Add(takenItems[i]);
			}
			else
				items.Add(takenItems[i]);
		}
		UpdateInventory();
	}
	
	void CalculateWeigth()
	{
		//TODO
		float weigth=0f;
		for(int i = 0; i<items.Count;i++)
		{
			
			weigth+=items[i].weigth*items[i].quantity;
			
		}
		total_weigth.text = weigth + " / " + "95" + "kg";
	}
	
	
	
}
