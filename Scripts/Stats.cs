using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour 
{
	//necessary gameObjects
	[SerializeField] protected Transform head;
	
	//UIs
	[SerializeField] protected Log log;
	[SerializeField] protected UIController UIControl;
	
	//personal info
	public string nickname;
	public Sprite portrait;
	[TextArea(4,3)] public string desc;

	//stats
	[HideInInspector] public int hp;
	public int maxHp;
	public int accuracy;
	public int strength;
	public int reflex;
	public int agility;
	
	//equipment
	[HideInInspector] public ItemWeapon weapon;
	[HideInInspector] public ItemArmor armor;
	[HideInInspector] public ItemHelmet helmet;
	
	protected AudioSource audioSource;
	
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		
		
		hp = maxHp;
	}
	
	public void Injury(int dmg, part bodyPart)
	{
		
		hp-=dmg;
		string stringPart = "";
		if(bodyPart==part.head)
		{
			stringPart = "head";
		}
		else if(bodyPart==part.chest)
		{
			stringPart = "chest";
		}
		else if(bodyPart==part.legs)
		{
			stringPart = "legs";
		}
		
		log.Send(nickname + " has been hit in "+stringPart+" lost " + dmg + " HP");
		
		if(hp<1)
		{
			log.Send(nickname + " has died");

			//TODO death
			GameController.RemoveFromList(gameObject);
			Destroy(gameObject);
		}
		
		UIControl.UIControl();
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
		helmet = (ItemHelmet)item;
		//armor f
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

		return i;
	}
	
}
