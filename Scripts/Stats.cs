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
	
	
	[HideInInspector] public float accuracyStateBonus = 1f;
	[HideInInspector] public float defenseStateBonus = 1f;
	[HideInInspector] public float accuracyModePenalty = 1f;
	
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
				ShootCheck();
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
				if(item.ammo == weapon.ammoUsed.ammo)
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
	public void ShootCheck()
	{
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
				ReloadWeapon();
				burnOut = 1.5f; //TODO reload cost and burn out
				
			}
			else
			{
				transform.LookAt(currentTarget); // look at
				sound.PlayEmpty(transform);
				burnOut = 1;
				SetTarget(null);
			}
			return;
		}
		
		
		if(weapon.mode == burstMode.burst && ap>=weapon.apCost*2.5f)
		{
			StartCoroutine(Burst());
			burnOut = 1.2f;
			ap-=weapon.apCost*2.5f;
		}
		else if(weapon.mode == burstMode.single && ap>=weapon.apCost)
		{
			SingleShoot();
			burnOut = 1;
			ap-=weapon.apCost;
		}
		else if(weapon.mode == burstMode.auto && ap>=weapon.apCost/5)
		{
			SingleShoot();
			burnOut = 60/weapon.rateOfFire;
			ap-=weapon.apCost/5;
		}
	}
	public IEnumerator Burst()
	{
		for(int i = 0; i < 3; i++)
		{
			if(weapon.bulletsLeft == 0)
			{
				sound.PlayEmpty(transform);
				break;
			}
			SingleShoot();
			yield return new WaitForSeconds(0.1f); //fire rate? TODO
		}
	}

	protected virtual void SingleShoot()
	{
		transform.LookAt(currentTarget);
		sound.PlayAtPoint(weapon.shootSound, transform);
		weapon.bulletsLeft--;
		weapon.weight -= weapon.ammoUsed.weight;
		float chanceToHit = Formulas.ChanceToHit(this, currentTarget);
		Vector3 lineDir;
		
		if(chanceToHit>=Random.Range(1,100))
		{
			lineDir = currentTarget.position - head.position;
		}
		else
		{
			Vector3 missedShot = Formulas.MissedShotRandomizer(chanceToHit, Formulas.Distance(head.transform, currentTarget));
			lineDir = currentTarget.position - head.position - missedShot;
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
			Death();
		}
	}
	
	protected void Death()
	{
		log.Send(nickname + " was killed");
		UnequipEverything();
		GameController.RemoveFromList(gameObject);
		GameObject dropped = Instantiate(drop, transform.position, Quaternion.identity);
		PickupItem d = dropped.GetComponent<PickupItem>();
		for(int i = 0; i < inv.items.Count; i++)
		{
			d.items.Add(inv.items[i]);
		}
		DeathAction();
		Destroy(gameObject);
	}
	
	protected virtual void DeathAction(){ }
	
	#region reload/eject/bursts
	public virtual void ReloadWeapon()
	{
		if(!weapon || weapon.bulletsLeft == weapon.capacity)
			return;
		
		if(ap<weapon.apCost)
			return;
		
		if(!inv.FindItemInInventory(weapon.ammoUsed.name))
			return;
		
		Item ammo = (ItemAmmo)inv.GetItem(weapon.ammoUsed.name);
		int need = weapon.capacity - weapon.bulletsLeft;
		int have = Mathf.Min(ammo.quantity, need);
		weapon.weight += (have * ammo.weight);
		weapon.bulletsLeft += have;
		ammo.quantity -= have;
		if(ammo.quantity==0)
		{
			inv.RemoveItem(ammo);
		}
		ap-=weapon.apCost;
		sound.PlayAtPoint(weapon.reloadSound, transform);
	}
	
	public virtual void EjectAmmo()
	{
		if(!weapon || weapon.bulletsLeft == 0)
			return;
		
		Item ammo = (ItemAmmo)inv.CreateItem(weapon.ammoUsed);
		
		ammo.quantity = weapon.bulletsLeft;
		weapon.weight -= (weapon.bulletsLeft*ammo.weight);
		weapon.bulletsLeft = 0;
		
		inv.AddItem(ammo);
		
		sound.PlayAtPoint(weapon.reloadSound, transform);
	}
	
	public void SwitchWeaponMode(burstMode mode)
	{
		weapon.mode = mode;
		
		if(mode == burstMode.single)
			accuracyModePenalty = 1f;	
		else if(mode == burstMode.burst)
			accuracyModePenalty = Formulas.accuracyBurstPenalty;
		else if(mode == burstMode.auto)
			accuracyModePenalty = Formulas.accuracyAutoPenalty;
		
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
		
		if(statePos == state.stand)
		{
			accuracyStateBonus = 1f;
			defenseStateBonus = 1f;
		}
		else if(statePos == state.crouch)
		{
			accuracyStateBonus = Formulas.accuracyCrouchBonus;
			defenseStateBonus = Formulas.defenseCrouchBonus;
		}
		else if(statePos == state.crawl)
		{
			accuracyStateBonus = Formulas.accuracyCrawlBonus;
			defenseStateBonus = Formulas.defenseCrawlBonus;
		}
	}
	
}




