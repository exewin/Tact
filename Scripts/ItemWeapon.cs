﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class ItemWeapon : Item 
{
	
	public int power;
	public int capacity;
	public int ammoLoaded;
	public int accuracy;
	public int apCost;
	public enum ammoType{a32, a358, a38, a44, a45, a50, a5_56, a7_62, a12_7};
	
	public enum burstMode{single, burst, auto}
	
	public AudioClip shootSound;
	
	
}
