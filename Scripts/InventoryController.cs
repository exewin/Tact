using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour 
{
	[HideInInspector] public List<GameObject> mercs = new List<GameObject>();
	[HideInInspector] public StatsMerc stats;
	
	[HideInInspector] public List<Item> items = new List<Item>();
	
	[SerializeField] private GameObject slotPrefab;
	private List<GameObject> backpackSlots = new List<GameObject>();
	[SerializeField] private Transform gridParent;
	
	[SerializeField] private UIController UIControl;
	
	//UI
	[SerializeField] private Text item_name;
	[SerializeField] private Image item_icon;
	[SerializeField] private Text item_desc;
	[SerializeField] private Text item_statA;
	[SerializeField] private Text item_stat1;
	[SerializeField] private Text item_stat2;
	[SerializeField] private Text item_stat3;
	[SerializeField] private Text item_stat4;
	[SerializeField] private Text item_stat5;
	[SerializeField] private Sprite transparent;
	[SerializeField] private Text total_weight;
	
	public Transform draggingArea;
	public RectTransform weaponArea;
	public RectTransform armorArea;
	public RectTransform helmetArea;
	public RectTransform[] mercArea = new RectTransform[6];
	
	
	private void Start()
	{
		ClearHover();
	}
	private void Update()
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
	
	#region UpdateInv
	public void UpdateInventory()
	{
		ClearSlots(); //delete existing slots
		CreateSlots(); //create new slots
		CalculateWeight(); //calculate weight
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
			slot.GetComponent<UIBackpackSlot>().Assign(this, items[i], i);
			backpackSlots.Add(slot);
		}
	}
	
	public void RemoveItem(int index)
	{
		items.RemoveAt(index);
		RemoveSlot(index);
	}
	
	private void RemoveSlot(int index)
	{
		Destroy(backpackSlots[index]);
		UpdateInventory();
	}
	
	private void CalculateWeight()
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
			
		total_weight.text = weight.ToString("F2") + " / " + Formulas.Weight(stats).ToString("F2") + "kg";
		
		if(weight > Formulas.Weight(stats))
		{
			//TODO
		}
	}
	#endregion
	
	public void GiveItem(int index, int toWho)
	{
		GameController.mercs[toWho].GetComponent<Inventory>().AddItem(items[index]);
	}
	
	public void TakeItem(List<Item> takenItems)
	{
		for(int i = 0; i < takenItems.Count; i++)
		{
			stats.GetComponent<Inventory>().AddItem(takenItems[i]);
		}
		UpdateInventory();
	}
	
	public void EquipItem(int index)
	{
		if(items[index] is ItemWeapon)
		{
			stats.EquipWeapon(items[index]);
		}
		else if(items[index] is ItemArmor)
		{
			stats.EquipArmor(items[index]);
		}
		else if(items[index] is ItemHelmet)
		{
			stats.EquipHelmet(items[index]);
		}		
		else if(items[index] is ItemAmmo)
		{
			ManualReload((ItemAmmo)(items[index]));
		}
		else //Other objects TODO
			return;
		
		RemoveSlot(index);
		UIControl.UIControl();
	}
	
	public void ManualReload(ItemAmmo ammo)
	{
		if(stats.weapon)
			if(stats.weapon.ammoUsed.ammo == ammo.ammo)
				ReloadWeaponButton();
	}
	
	#region buttons
	public void UnequipWeaponButton()
	{
		if(stats.weapon)
		{
			stats.UnequipWeapon();
			UpdateInventory();
			UIControl.UIControl();
		}
	}
		
	public void UnequipArmorButton()
	{
		if(stats.armor)
		{
			stats.UnequipArmor();
			UpdateInventory();
			UIControl.UIControl();
		}
	}
		
	public void UnequipHelmetButton()
	{
		if(stats.helmet)
		{
			stats.UnequipHelmet();
			UpdateInventory();
			UIControl.UIControl();
		}
	}
	
	public void EjectAmmoButton()
	{
		stats.EjectAmmo();
		UpdateInventory();
	}
	public void ReloadWeaponButton()
	{
		stats.ReloadWeapon();
		UpdateInventory();
	}
	
	public void BurstModeButton(int mode)
	{
		stats.SwitchWeaponMode((burstMode)mode);
		UIControl.UIControl();
	}
	
	public void StateButton(int mode)
	{
		stats.SwitchState((state)mode);
		UIControl.UIControl();
	}
	#endregion
	
	public void HoverItemInfo(Item info)
	{
		ClearHover();
		
		item_name.text = info.name;
		item_icon.sprite = info.image;
		item_desc.text = info.desc;
		item_stat1.text = "Weight: "+(info.weight*info.quantity).ToString("F2")+" kg";
		
		if(info.stackable)
			item_statA.text = ""+info.quantity;
		
		
		if(info is ItemWeapon)
		{
			ItemWeapon weapon = (ItemWeapon) info;
			item_statA.text = weapon.bulletsLeft +" / "+weapon.capacity;
			item_stat2.text = "Caliber: "+ConvertAmmoTypeToString(weapon.ammoUsed.ammo);
			item_stat3.text = "Power: "+weapon.power;
			item_stat4.text = "Accuracy Bonus: "+weapon.accuracy+"%";
			item_stat5.text = "Velocity: "+weapon.velocity+"m/s";
		}
	}
	
	private void ClearHover()
	{
		item_name.text = "";
		item_icon.sprite = transparent;
		item_desc.text = "";
		item_statA.text = "";
		item_stat1.text = "";
		item_stat2.text = "";
		item_stat3.text = "";
		item_stat4.text = "";
		item_stat5.text = "";
	}
	
	private string ConvertAmmoTypeToString(ammoType ammo)
	{
		
		if(ammo==ammoType.a_32_ACP)
			return ".32 ACP";
		else if(ammo==ammoType.a_357_Magnum)
			return ".357 Magnum";
		else if(ammo==ammoType.a_38_Special)
			return ".38 Special";
		else if(ammo==ammoType.a_44_Magnum)
			return ".44 Magnum";
		else if(ammo==ammoType.a_45_ACP)
			return ".45 ACP";
		else if(ammo==ammoType.a_50AE)
			return ".50 AE";
		else if(ammo==ammoType.a_50BMG)
			return ".50 BMG";
		else if(ammo==ammoType.a_12_Gauge)
			return "12 Gauge";
		else if(ammo==ammoType.a_20_Gauge)
			return "20 Gauge";
		else if(ammo==ammoType.a_5_45x39mm)
			return "5.45x39mm";
		else if(ammo==ammoType.a_5_56x45mm)
			return "5.56x45mm";
		else if(ammo==ammoType.a_7_62x25mmTokarev)
			return "7.62x25mm Tokarev";
		else if(ammo==ammoType.a_7_62x39mm)
			return "7.62x39mm";
		else if(ammo==ammoType.a_7_62x51mm)
			return "7.62x51mm";
		else if(ammo==ammoType.a_7_62x54mmR)
			return "7.62x54mmR";
		else if(ammo==ammoType.a_9x19mmParabellum)
			return "9x19mm Parabellum";
		else
			return "unknown";
	}



	
}
