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
	
}

	public enum burstMode{single, burst, auto}