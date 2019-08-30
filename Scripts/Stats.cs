using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum state{ stand, crouch, crawl }

public class Stats : MonoBehaviour 
{
	//necessary gameObjects
	public Transform head;
	[SerializeField] private GameObject drop;
	[SerializeField] private GameObject shot;
	protected Inventory inv;
	[SerializeField] protected Visibility vis;
	
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
	
	public float ap;
	public int maxAp;
	
	private Transform currentTarget;
	private float burnOut = 0;
	
	[HideInInspector] public state statePos;
	
	//equipment
	[HideInInspector] public ItemWeapon weapon;
	[HideInInspector] public ItemArmor armor;
	[HideInInspector] public ItemHelmet helmet;
	
	//controllers
	protected AudioSource audioSource;
	protected LogController log;
	protected SoundController sound;
	
	protected void Awake()
	{
		log = GameObject.Find("LOG CONTROLLER").GetComponent<LogController>();
		sound = GameObject.Find("SOUND CONTROLLER").GetComponent<SoundController>();
		audioSource = GetComponent<AudioSource>();
		inv = GetComponent<Inventory>();
		hp = maxHp;
	}
	
	protected virtual void Update()
	{
		if(burnOut>0)
			burnOut-=Time.deltaTime*1;
		
		if(currentTarget)
		{
			if(burnOut<=0)
				ShootCheck(currentTarget);
		}
		
		if(ap<maxAp)
			ap+=Time.deltaTime*10; //? TODO
	}
	
	public void SetTarget(Transform target)
	{
		currentTarget = target;
	}
	
	#region equip/unequip
	public void EquipWeapon(Item item)
	{
		if(weapon)
			UnequipWeapon();
		
		inv.RemoveItem(item);
		weapon = (ItemWeapon)item;
		FindAmmo();
	}
	
	public void EquipArmor(Item item)
	{
		if(armor)
			UnequipArmor();
		
		inv.RemoveItem(item);
		armor = (ItemArmor)item;
	}
	
	public void EquipHelmet(Item item)
	{
		if(helmet)
			UnequipHelmet();
		
		inv.RemoveItem(item);
		helmet = (ItemHelmet)item;
	}
	
	public void FindAmmo()
	{
		for(int i = 0; i < inv.items.Count; i++)
		{
			if(inv.items[i] is ItemAmmo)
			{
				ItemAmmo item = (ItemAmmo)inv.items[i];
				if(item.ammo == weapon.ammo)
				{
					weapon.ammoUsed = item;
				}
			}
		}
	}
	
	public void UnequipWeapon()
	{
		if(weapon==null)
			return;
		
		inv.AddItem(weapon);
		weapon=null;
	}	
	public void UnequipArmor()
	{
		if(armor==null)
			return;
		
		inv.AddItem(armor);
		armor=null;
	}	
	public void UnequipHelmet()
	{
		if(helmet==null)
			return;
		
		inv.AddItem(helmet);
		helmet=null;
	}
	
	public void UnequipEverything()
	{
		UnequipArmor();
		UnequipHelmet();
		UnequipWeapon();
	}
	#endregion
	
	
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
	
	
	
	#region shooting
	public void ShootCheck(Transform target)
	{
		float accuracyModifer = 1;
		if(!weapon)
		{
			Debug.Log("Merc has no weapon!"); //?
			SetTarget(null);
			return;
		}
		if(weapon.bulletsLeft == 0)
		{
			if(weapon.ammoUsed && weapon.ammoUsed.quantity>0)
			{
				ReloadWeapon(weapon.ammoUsed);
				burnOut = 1.5f; //TODO reload cost and burn out
				
			}
			else
			{
				transform.LookAt(target); // look at
				sound.PlayEmpty(transform);
				burnOut = 1;
				SetTarget(null);
			}
			return;
		}
		if(weapon.mode == burstMode.burst && ap>=weapon.apCost*2.5f)
		{
			StartCoroutine(Burst(target,accuracyModifer));
			burnOut = 1.2f;
			ap-=weapon.apCost*2.5f;
			accuracyModifer = 0.8f;
		}
		else if(weapon.mode == burstMode.single && ap>=weapon.apCost)
		{
			SingleShoot(target,accuracyModifer);
			burnOut = 1;
			ap-=weapon.apCost;
		}
		else if(weapon.mode == burstMode.auto && ap>=weapon.apCost/5)
		{
			SingleShoot(target,accuracyModifer);
			burnOut = 60/weapon.rateOfFire;
			ap-=weapon.apCost/5;
			accuracyModifer = 0.2f;
		}
	}
	public IEnumerator Burst(Transform target, float accuracyModifer)
	{
		for(int i = 0; i < 3; i++)
		{
			if(weapon.bulletsLeft == 0)
			{
				sound.PlayEmpty(transform);
				break;
			}
			SingleShoot(target,accuracyModifer);
			yield return new WaitForSeconds(0.1f); //fire rate? TODO
		}
	}
	
	protected virtual void SingleShoot(Transform target, float accuracyModifer)
	{
		
		int accuracyFlat = 0;
		if(statePos == state.stand)
		{
			accuracyFlat = 0;
		}
		else if(statePos == state.crouch)
		{
			accuracyFlat = Formulas.crouchBonus;
		}
		else if(statePos == state.crawl)
		{
			accuracyFlat = Formulas.crawlBonus;
		}
		
		transform.LookAt(target);
		sound.PlayAtPoint(weapon.shootSound, transform);
		weapon.bulletsLeft--;
		weapon.weight -= weapon.ammoUsed.weight;
		float distance = Formulas.Distance(head, target);
		float chanceToHit = Formulas.ChanceToHit(distance, (int)(accuracy*accuracyModifer+accuracyFlat), weapon, target.GetComponent<BodyPart>().bodyPart);
		Vector3 lineDir;
		
		if(chanceToHit>=Random.Range(1,100))
		{
			lineDir = target.position - head.position;
		}
		else
		{
			Vector3 missedShot = Formulas.MissedShotRandomizer(chanceToHit, distance);
			lineDir = target.position - head.position - missedShot;
		}
		GameObject s = Instantiate(shot, head.transform.position, transform.rotation);
		s.GetComponent<RealShoot>().Info(lineDir, weapon);
	}
	#endregion
		
	public virtual void Injury(int dmg, part bodyPart)
	{ //wsadz to w formule TODO
		float dmgMultiplier = 1f; 
		float armorDecreaser = 1f;
		int armorFlat = 0;
		string stringPart = "";
		if(bodyPart==part.head)
		{
			stringPart = "head";
			dmgMultiplier = Formulas.headDmgMultiplier;
			if(helmet)
			{
				armorDecreaser = Formulas.DefenseFormula(helmet.defense, dmg);
				armorFlat = helmet.flatDefense;
			}
		}
		else if(bodyPart==part.chest)
		{
			stringPart = "chest";
			if(armor)
			{
				armorDecreaser = Formulas.DefenseFormula(armor.defense, dmg);
				armorFlat = armor.flatDefense;
			}
		}
		else if(bodyPart==part.legs)
		{
			stringPart = "legs";
			dmgMultiplier = Formulas.legsDmgMultiplier;
			if(armor)
			{
				if(armor.protectLegs)
				{
					armorDecreaser = Formulas.DefenseFormula(armor.defense, dmg);
					armorFlat = armor.flatDefense;
				}
			}
		}
		
		dmg = Formulas.DamageFormula(dmg, dmgMultiplier, armorDecreaser, armorFlat);
		
		hp-=dmg;
		log.Send(nickname + " was hit in the "+stringPart+" for " + dmg + " HP");
		
		if(hp<1)
		{
			log.Send(nickname + " was killed");

			//TODO death
			UnequipEverything();
			GameController.RemoveFromList(gameObject);
			GameObject dropped = Instantiate(drop, transform.position, Quaternion.identity);
			PickupItem d = dropped.GetComponent<PickupItem>();
			for(int i = 0; i < inv.items.Count; i++)
			{
				d.items.Add(inv.items[i]);
			}
			
			
			Destroy(gameObject);
		}
	}
	
	#region reload/eject/bursts
	public virtual void ReloadWeapon(ItemAmmo ammo)
	{
		if(ap>=weapon.apCost)
		{
			if(!weapon)
				return;
			
			if(ammo.ammo != weapon.ammo)
				return;
			
			weapon.ammoUsed = ammo;
			int need = weapon.capacity - weapon.bulletsLeft;
			int have = Mathf.Min(ammo.quantity,need);
			weapon.weight += (have*ammo.weight);
			weapon.bulletsLeft += have;
			ammo.quantity -= have;
			if(ammo.quantity==0)
			{
				ammo = null;
			}
			ap-=weapon.apCost;
			sound.PlayAtPoint(weapon.reloadSound, transform);
		}
	}
	
	public virtual void EjectAmmo(ItemAmmo ammo)
	{
		if(!weapon)
			return;
		
		if(ammo.ammo!=weapon.ammo)
			return;
		
		ammo.quantity += weapon.bulletsLeft;
		weapon.weight -= (weapon.bulletsLeft*ammo.weight);
		weapon.bulletsLeft = 0;
		
		sound.PlayAtPoint(weapon.reloadSound, transform);
	}
	
	public void SwitchWeaponMode(burstMode mode)
	{
		weapon.mode = mode;
	}
	#endregion
	
	public void SwitchState(state mode)
	{
		if(statePos != mode)
		{
			statePos = mode;
			vis.BodyPartsResize(mode);
			//movement? TODO
		}
	}
	
}




