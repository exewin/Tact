using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMerc : Stats
{
	private int id;
	[SerializeField] private UIController UIControl;
	

	public void SetID(int i)
	{
		id = i;
	}
	
	protected override void Update()
	{
		base.Update();
		UIControl.UIAp();
	}

	public override void ReloadWeapon()
	{
		base.ReloadWeapon();
		UIControl.UIControl();
	}
	
	public override void EjectAmmo()
	{
		base.EjectAmmo();
		UIControl.UIControl();
	}
	
	public override void Injury(int dmg, part bodyPart)
	{
		base.Injury(dmg, bodyPart);
		UIControl.UIControl();
	}
	
	protected override void DeathAction()
	{
		UIControl.MercDeath(id);
	}
	
	protected override void SingleShoot()
	{
		base.SingleShoot();
		UIControl.UIControl();
	}
	
	
	
}
