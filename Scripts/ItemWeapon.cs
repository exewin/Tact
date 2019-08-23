using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class ItemWeapon : Item 
{
	
	public int power = 30;
	public int capacity = 30;
	[HideInInspector] public int bulletsLeft = 0;
	public int accuracy = 0;
	public int apCost = 20;
	
	public ammoType ammo;
	
	public float velocity = 400;
	public float rateOfFire = 40;
	public float effectiveRange = 350;
	
	public AudioClip shootSound;
	public AudioClip reloadSound;
	
	public weaponType type;
	
	public burstMode mode;
	public bool single = true;
	public bool burst;
	public bool auto;
	
	[HideInInspector] public ItemAmmo ammoUsed; 
	public GameObject trace;
	
}

	public enum burstMode{single, burst, auto}
	public enum weaponType{pistol, revolver, submachine_pistol, rifle, shotgun, assault_rifle}