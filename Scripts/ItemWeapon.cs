using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class ItemWeapon : Item 
{
	
	public int power;
	public int capacity;
	[HideInInspector] public int bulletsLeft;
	public int accuracy;
	public int apCost;
	
	public ammoType ammo;
	
	public float velocity;
	
	public AudioClip shootSound;
	public AudioClip reloadSound;
	
	public weaponType type;
	
	public burstMode mode;
	public bool single = true;
	public bool burst;
	public bool auto;
	
	[HideInInspector] public ItemAmmo ammoUsed; 
	
}

	public enum burstMode{single, burst, auto}
	public enum weaponType{pistol, revolver, submachine_pistol, rifle, shotgun, assault_rifle}