using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour 
{
	[HideInInspector]
	public List<Item> items = new List<Item>();
	[HideInInspector]
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
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			ReloadWeaponButton();
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			EjectAmmoButton();
		}
	}
	
	
	public void GetMerc(GameObject merc)
	{
		items = merc.GetComponent<Inventory>().items;
		stats = merc.GetComponent<StatsMerc>();
		UpdateInventory();
	}	
	
	public void UpdateInventory()
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
			slot.transform.SetParent(gridParent,false);
			slot.GetComponent<UIBackpackSlot>().Assign(items[i], i);
			backpackSlots.Add(slot);
		}
	}
	
	public void RemoveItem(int index)
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
			ItemAmmo tmp = FindAmmoInInventory(stats.weapon.ammo);
			if(tmp!=null)
			{
				stats.weapon.ammoUsed = tmp;
			}
		}
		else if(items[index] is ItemArmor)
		{
			if(stats.armor)
			{
				UnequipItem(stats.armor);
			}
			stats.EquipArmor(items[index]);
		}
		else if(items[index] is ItemHelmet)
		{
			if(stats.helmet)
			{
				UnequipItem(stats.helmet);
			}
			stats.EquipHelmet(items[index]);
		}
		else
			return;
		
		RemoveItem(index);
		UIControl.UIControl();
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
		if(stats.weapon&&stats.weapon.ammoUsed!=null && stats.weapon.bulletsLeft > 0)
		{
			stats.EjectAmmo(stats.weapon.ammoUsed);
			UpdateInventory();
		}
	}
	public void ReloadWeaponButton()
	{
		if(stats.weapon&&stats.weapon.ammoUsed!=null && stats.weapon.ammoUsed.quantity > 0)
		{
			stats.ReloadWeapon(stats.weapon.ammoUsed);
			UpdateInventory();
		}
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
	
	private ItemAmmo FindAmmoInInventory(ammoType s)
	{
		for(int i = 0; i < items.Count; i++)
		{
			if(items[i] is ItemAmmo)
			{
				ItemAmmo item = (ItemAmmo)items[i];
				if(item.ammo == s)
				{
					return item;
				}
			}
		}
		return null;
	}	
	
	private void Calculateweight()
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
				weight+=equiped[i].weight;
			}
			
		total_weight.text = weight + " / " + Formulas.Weight(stats) + "kg"; //F2? TODO
		
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
