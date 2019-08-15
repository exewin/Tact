using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMerc : Stats
{
	
	[SerializeField] private UIController UIControl;

	public override void ReloadWeapon(ItemAmmo ammo)
	{
		base.ReloadWeapon(ammo);
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
	
	public override void SingleShoot(Transform target)
	{
		base.SingleShoot(target);
		UIControl.UIControl();
	}
	
	
}
