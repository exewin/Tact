using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour 
{
	
	
	
	//personal info
	public string nickname;
	public Sprite portrait;
	public string desc;

	//stats
	public int hp;
	public int maxHp;
	public int accuracy;
	public int strength;
	public int reflex;
	public int agility;
	
	//equipment
	public ItemWeapon weapon;
	public ItemArmor armor;
	public ItemArmor helmet;
	public ItemAmmo ammo;
	
	protected AudioSource audioSource;
	
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}
	
	
	
	
	public void EquipWeapon(Item item)
	{
		weapon = (ItemWeapon)item;
		//dmg f
	}
	
	public void EquipArmor(Item item)
	{
		armor = (ItemArmor)item;
		//armor f
	}
	
	public void EquipHelmet(Item item)
	{
		helmet = (ItemArmor)item;
		//armor f
	}
	
	public void EquipAmmo(Item item)
	{
		ammo = (ItemAmmo)item;
		//dmg f
	}
	
	public List<Item> ReturnItems()
	{
		List <Item> i = new List<Item>();
		if(weapon)
			i.Add(weapon);
		if(armor)
			i.Add(armor);
		if(helmet)
			i.Add(helmet);
		if(ammo)
			i.Add(ammo);
		return i;
	}
	
}
