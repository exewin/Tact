using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHostile : Stats 
{

	protected override void Awake()
	{
		base.Awake();
		AutoEquipLazy();
	}
	
	private void AutoEquipLazy()
	{
		//large bug was here
		//script was operating on items.Count on for loop.
		//the problem is every time item was equiped, it get removed
		//from list which caused Count to shrink thus loop was skipping some items
		int itemsLazy = inv.items.Count; 
		for(int i = itemsLazy-1; i>=0;i--)
		{
			if(inv.items[i] is ItemWeapon)
			{
				EquipWeapon(inv.items[i]);
				ReloadWeapon();
				continue;
			}
			if(inv.items[i] is ItemHelmet)
			{
				EquipHelmet(inv.items[i]);
				continue;
			}
			if(inv.items[i] is ItemArmor)
			{
				EquipArmor(inv.items[i]);
				continue;
			}
		}
	}

	public override void ShootCheck()
	{
		base.ShootCheck();
		if(!weapon)
		{
			AutoEquipLazy();
		}
	}

}
