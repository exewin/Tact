using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour 
{

	public List<Item> items = new List<Item>();
	public StatsMerc stats;
	public GameObject slotPrefab;
	public Transform gridParent;
	List<GameObject> backpackSlots = new List<GameObject>();
	public UIController UIControl;
	
	bool autoEquipAmmo;
	
	
	//UI
	public Text item_name;
	public Image item_icon;
	public Text item_desc;
	public Text item_stat1;
	public Text item_stat2;
	public Text item_stat3;
	public Text item_stat4;
	
	public Text total_weight;
	
	
	public void GetMerc(GameObject merc)
	{
		items = merc.GetComponent<Inventory>().items; //reference instead of creating new list
		stats = merc.GetComponent<StatsMerc>();
		UpdateInventory();
	}	
	
	private void UpdateInventory()
	{
		ClearSlots(); //delete existing slots
		CreateSlots(); //create new slots
		Calculateweight(); //calculate weight
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
			slot.GetComponent<UIBackpackSlot>().Assign(items[i], i);
			backpackSlots.Add(slot);
		}
	}
	
	public void DropItem(int index)
	{
		items.RemoveAt(index);
		Destroy(backpackSlots[index]);
		UpdateInventory();
		//TODO
		//spawn gameobject on scene
	}
	
	public void TakeItem(List<Item> takenItems)
	{
		for(int i = 0; i < takenItems.Count; i++)
		{
			if(takenItems[i].stackable)
			{
				if(FindItemInInventory(takenItems[i].name))
				{
					for(int j = 0; j < items.Count; j++)
					{
						if(items[j].name == takenItems[i].name)
						{
							items[j].quantity+=takenItems[i].quantity;
							break;
						}
					}
				}
				else
				{
					Item j = Instantiate(takenItems[i]);
					items.Add(j);
				}
			}
			else
			{
				Item j = Instantiate(takenItems[i]);
				items.Add(j);
			}
		}
		UpdateInventory();
	}
	
	public void EquipItem(int index)
	{
		if(items[index] is ItemWeapon)
		{
			if(stats.weapon)
			{
				UnequipItem(stats.weapon);
			}
			stats.EquipWeapon(items[index]);
			DropItem(index);
			UIControl.UIControl();
			//auto equip ammo TODO
		}
		else if(items[index] is ItemArmor)
		{
			if(stats.armor)
			{
				UnequipItem(stats.armor);
			}
			stats.EquipArmor(items[index]);
			DropItem(index);
			UIControl.UIControl();
		}
		else if(items[index] is ItemHelmet)
		{
			if(stats.helmet)
			{
				UnequipItem(stats.helmet);
			}
			stats.EquipHelmet(items[index]);
			DropItem(index);
			UIControl.UIControl();
		}
			
		else if(items[index] is ItemAmmo)
		{
			if(stats.ammo)
			{
				UnequipItem(stats.ammo);
			}
			stats.EquipAmmo(items[index]);
			DropItem(index);
			UIControl.UIControl();
		}
	}
	
	public void UnequipItem(Item item)
	{
		if(item)
		{
			items.Add(item);
		}
	}
	
	public void EjectAmmoButton()
	{
		stats.EjectAmmo();
	}
	public void ReloadWeaponButton()
	{
		stats.ReloadWeapon();
	}
	
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
	
	void Calculateweight()
	{
		float weight=0f;
		for(int i = 0; i<items.Count;i++)
		{
			weight+=items[i].weight*items[i].quantity;
		}
		List<Item> equiped = stats.ReturnItems();
		
		if(equiped.Count!=0)
			for(int i = 0; i<equiped.Count;i++)
			{
				weight+=equiped[i].weight*equiped[i].quantity;
			}
			
		total_weight.text = weight + " / " + Formulas.Weight(stats) + "kg";
		
		if(weight > Formulas.Weight(stats))
		{
			//TODO
			Debug.Log(stats.nickname+" is overloaded");
		}
	}
	
	public void HoverItemInfo(Item info)
	{
		item_name.text = info.name;
		item_icon.sprite = info.image;
		item_desc.text = info.desc;
	}
	
	
	
}
