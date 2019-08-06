using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMerc : Stats
{
	public void Shoot()
	{
		if(ammo.quantity>0)
		{
			audioSource.PlayOneShot(weapon.shootSound);
			ammo.quantity--; //DO THIS IN INV CONTROLLER
		}
	}
	
	public int StrengthFormula()
	{
		return strength;
	}
}
