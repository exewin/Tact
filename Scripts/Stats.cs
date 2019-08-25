using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour 
{
	//necessary gameObjects
	public Transform head;
	[SerializeField] private GameObject drop;
	protected Inventory inv;
	
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
	
	//equipment
	[HideInInspector] public ItemWeapon weapon;
	[HideInInspector] public ItemArmor armor;
	[HideInInspector] public ItemHelmet helmet;
	
	protected AudioSource audioSource;
	[SerializeField] private AudioClip emptyGun;
	
	//UIs
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
	
	protected void Update()
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
				ap-=weapon.apCost;
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
		transform.LookAt(target);
		sound.PlayAtPoint(weapon.shootSound, transform);
		weapon.bulletsLeft--;
		weapon.weight -= weapon.ammoUsed.weight;
		float distance = Formulas.Distance(head, target);
		float chanceToHit = Formulas.ChanceToHit(distance, (int)(accuracy*accuracyModifer), weapon, target.GetComponent<BodyPart>().bodyPart);
		Color col;
		Vector3 lineDir;
		bool aimed;
		
		if(chanceToHit>=Random.Range(1,100))
		{
			col = Color.green;
			lineDir = target.position - head.position;
			aimed = true;
		}
		else
		{
			col = Color.yellow;
			Vector3 missedShot = new Vector3(Random.Range(-1f,1f),Random.Range(-0.3f,0.3f),Random.Range(-1f,1f));
			lineDir = target.position - head.position - missedShot;
			aimed = false;
		}
		StartCoroutine(ShotDelay(lineDir,distance));
	}
	
	private IEnumerator ShotDelay(Vector3 lineDir,float distance)
	{
		GameObject line = Instantiate(weapon.trace,head.position,transform.rotation);
		line.transform.forward = lineDir;
		line.GetComponent<Trace>().Info(weapon.velocity/Formulas.VELOCITY_SCALER);
		RaycastHit hit;
		float realDistance = 0;
		
		//real distance ray
		if(Physics.Raycast(head.position, lineDir, out hit))
		{
			realDistance = Vector3.Distance(head.position,hit.point);
			yield return new WaitForSeconds(realDistance/(weapon.velocity/Formulas.VELOCITY_SCALER));
		}
		
		if(Physics.Raycast(head.position, lineDir, out hit))
		{
			if(hit.collider.tag=="Shootable")
			{
				
				//if(!aimed)
					//Debug.Log("lucky shot"); //LOG?
				
				hit.collider.GetComponent<BodyPart>().Hit(Formulas.EffectiveRange(weapon,realDistance*2));
			}
			Destroy(line);
		}
	}
	#endregion
		
	public virtual void Injury(int dmg, part bodyPart)
	{
		float dmgMultiplier = 1f; 
		float armorDecreaser = 1f;
		int armorFlat = 0;
		string stringPart = "";
		if(bodyPart==part.head)
		{
			stringPart = "head";
			dmgMultiplier = 2f;
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
			dmgMultiplier = 0.3f;
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
		
		sound.PlayAtPoint(weapon.reloadSound, transform);
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
}
