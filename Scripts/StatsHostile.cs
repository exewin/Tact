using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHostile : Stats 
{

	protected void Awake()
	{
		base.Awake();
		AutoEquipLazy();
	}
	
	private void AutoEquipLazy()
	{
		for(int i = 0; i<inv.items.Count;i++)
		{
			if(inv.items[i] is ItemWeapon)
				EquipWeapon(inv.items[i]);
			else if(inv.items[i] is ItemHelmet)
				EquipHelmet(inv.items[i]);
			else if(inv.items[i] is ItemArmor)
				EquipArmor(inv.items[i]);
		}
	}

	protected override void SingleShoot()
	{
		base.SingleShoot();
		if(weapon.bulletsLeft == 0);
		{
			AutoEquipLazy();
		}
	}
}
