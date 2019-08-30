﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMerc : Stats
{
	
	[SerializeField] private UIController UIControl;
	
	protected override void Update()
	{
		base.Update();
		UIControl.UIAp();
	}

	public override void ReloadWeapon(ItemAmmo ammo)
	{
		base.ReloadWeapon(ammo);
		if(ap<weapon.apCost)
		{
			//sound :)
		}
		UIControl.UIControl();
	}
	
	public override void EjectAmmo(ItemAmmo ammo)
	{
		base.EjectAmmo(ammo);
		UIControl.UIControl();
	}
	
	public override void Injury(int dmg, part bodyPart)
	{
		base.Injury(dmg, bodyPart);
		if(hp<1)
		{
			UIControl.RemoveMerc(gameObject);
		}
		UIControl.UIControl();
	}
	
	protected override void SingleShoot(Transform target, float accuracyModifer)
	{
		base.SingleShoot(target, accuracyModifer);
		UIControl.UIControl();
	}
	
	
}
