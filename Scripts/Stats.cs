using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum state{ stand, crouch, crawl }

public class Stats : MonoBehaviour 
{
	
	
	//necessary gameObjects
	public Transform shootPoint;
	[SerializeField] private GameObject drop;
	[SerializeField] private GameObject shot;
	protected Inventory inv;
	protected Visibility vis;
	[HideInInspector] public NavMeshAgent navMeshAgent; //change this later
	
	
	
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
	
	private float chanceToHit = 0;
	[HideInInspector] public  Transform currentTarget;
	private Vector3 currentTargetPoint;
	private float burnOut = 0;
	
	
	[HideInInspector] public state statePos;
	
	[HideInInspector] public Vector3 destination;
	private Vector3 previousPosition;
	private float curSpeed;
	private bool isShooting;
	private bool hwequiped;
	
	//equipment
	[HideInInspector] public ItemWeapon weapon;
	[HideInInspector] public ItemArmor armor;
	[HideInInspector] public ItemHelmet helmet;
	
	//controllers
	protected AudioSource audioSource;
	protected LogController log;
	protected SoundController sound;
	protected Animator animator;
	
	[HideInInspector] public float accuracyStateBonus = 1f;
	[HideInInspector] public float defenseStateBonus = 1f;
	[HideInInspector] public float accuracyModePenalty = 1f;
	
	[SerializeField] private GameObject helmetModel;
	[SerializeField] private GameObject armorModel;
	[SerializeField] private GameObject lwModel;
	[SerializeField] private GameObject hwModel;
	
	private int nextAction = 255;
	//0posedown
	//1poseup
	//2move
	//3reload
	//4shoot
	
	protected virtual void Awake()
	{
		vis = gameObject.GetComponent<Visibility>();
		animator = gameObject.GetComponentInChildren<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		log = GameObject.Find("LOG CONTROLLER").GetComponent<LogController>();
		sound = GameObject.Find("SOUND CONTROLLER").GetComponent<SoundController>();
		audioSource = GetComponent<AudioSource>();
		inv = GetComponent<Inventory>();
		hp = maxHp;
	}
	
	protected virtual void Update()
	{
		if(burnOut>0)
		{
			burnOut-=Time.deltaTime*1;
		}
		else
			DoAction();
		
		
		if(ap<maxAp)
			ap+=Time.deltaTime*10; //? TODO
		
		Vector3 curMove = transform.position - previousPosition;
		curSpeed = curMove.magnitude / Time.deltaTime;
		animator.SetFloat("speed", curSpeed);
		previousPosition = transform.position;
		
		if(curSpeed>0&&currentTarget&&Time.time<0.1) //first shot blockade
		{
			SetTarget(null);
		}
		
		if(!currentTarget && isShooting)
		{
			SetTarget(null);
		}
		
		
	}
	
	public void SetTarget(Transform target)
	{
		if(target==null)
		{
			animator.SetBool("isShooting",false);
			isShooting = false;
		}
		else
		{
			ResetPath();
			animator.SetBool("isShooting",true);
			isShooting = true;
			burnOut=0.5f;
			SendAction(4);
		}
		
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
		
		if(weapon&&weapon.type != weaponType.pistol)
		{
			animator.SetBool("HWequiped", true);
			hwequiped = true;
			hwModel.SetActive(true);
		}
		else
		{
			animator.SetBool("HWequiped", false);
			hwequiped = false;
			lwModel.SetActive(true);
		}
	}
	
	public void EquipArmor(Item item)
	{
		if(armor)
			UnequipArmor();
		
		inv.RemoveItem(item);
		armor = (ItemArmor)item;
		armorModel.SetActive(true);
	}
	
	public void EquipHelmet(Item item)
	{
		if(helmet)
			UnequipHelmet();
		
		inv.RemoveItem(item);
		helmet = (ItemHelmet)item;
		helmetModel.SetActive(true);
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
		animator.SetBool("HWequiped", false);
		if(weapon.type != weaponType.pistol)
			hwModel.SetActive(false);
		else
			lwModel.SetActive(false);
		
		weapon=null;
	}	
	public void UnequipArmor()
	{
		if(armor==null)
			return;
		
		inv.AddItem(armor);
		armorModel.SetActive(false);
		armor=null;
	}	
	public void UnequipHelmet()
	{
		if(helmet==null)
			return;
		
		inv.AddItem(helmet);
		helmetModel.SetActive(false);
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
	public virtual void ShootCheck()
	{
		
		if(!weapon)
		{
			SetTarget(null);
			return;
		}
		if(weapon.bulletsLeft == 0)
		{
			if(weapon.ammoUsed && weapon.ammoUsed.quantity>0)
			{
				ReloadWeapon();
			}
			else
			{
				transform.LookAt(currentTargetPoint); // look at
				sound.PlayEmpty(transform);
				animator.SetTrigger("Shoot");
				burnOut = 1;
				SetTarget(null);
			}
			return;
		}
		
		if(currentTarget)
			currentTargetPoint = currentTarget.position;
		
		destination = new Vector3(0,0,0);
		
		
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
		else 
			return;
		
	}
	public IEnumerator Burst()
	{
		for(int i = 0; i < 3; i++)
		{	
			if(!weapon)
				break;
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
		
		
		if(currentTarget)
			chanceToHit = Formulas.ChanceToHit(this, currentTarget);
		
		transform.LookAt(currentTargetPoint);
		sound.PlayAtPoint(weapon.shootSound, transform);
		weapon.bulletsLeft--;
		weapon.weight -= weapon.ammoUsed.weight;
		
		Vector3 lineDir;
		
		animator.SetTrigger("Shoot");
		
		if(chanceToHit>=Random.Range(1,100))
		{
			lineDir = currentTargetPoint - shootPoint.position;
		}
		else
		{
			Vector3 missedShot = Formulas.MissedShotRandomizer(chanceToHit);
			lineDir = currentTargetPoint - shootPoint.position - missedShot;
		}
		GameObject s = Instantiate(shot, shootPoint.position, transform.rotation);
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
			dmgMultiplier = Formulas.HEAD_DMG_MULTIPLIER;
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
			dmgMultiplier = Formulas.LEG_DMG_MULTIPLIER;
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
		hp=0;
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
		{
			log.Send("Not enough AP ("+weapon.apCost+")");
			return;
		}
		
		if(!inv.FindItemInInventory(weapon.ammoUsed.name))
			return;
		
		
		
		if(statePos == state.stand && hwequiped)
			burnOut = Formulas.STAND_HEAVY_RELOAD_TIME;
		else if(statePos == state.stand && !hwequiped)
			burnOut = Formulas.STAND_LIGHT_RELOAD_TIME;
		else if(statePos == state.crouch && hwequiped)
			burnOut = Formulas.CROUCH_HEAVY_RELOAD_TIME;
		else if(statePos == state.crouch && !hwequiped)
			burnOut = Formulas.CROUCH_LIGHT_RELOAD_TIME;
		else if(statePos == state.crawl && hwequiped)
			burnOut = Formulas.CRAWL_HEAVY_RELOAD_TIME;
		else if(statePos == state.crawl && !hwequiped)
			burnOut = Formulas.CRAWL_LIGHT_RELOAD_TIME;
		
		ResetPath();
		animator.SetTrigger("Reload");
		
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
		if(!weapon)
			return;
		
		if(mode == burstMode.single && !weapon.single)
			return;
		
		if(mode == burstMode.burst && !weapon.burst)
			return;
		
		if(mode == burstMode.auto && !weapon.auto)
			return;
		
		weapon.mode = mode;
		
		if(mode == burstMode.single)
			accuracyModePenalty = 1f;	
		else if(mode == burstMode.burst)
			accuracyModePenalty = Formulas.ACCURACY_BURST_PENALTY;
		else if(mode == burstMode.auto)
			accuracyModePenalty = Formulas.ACCURACY_AUTO_PENALTY;
		
	}
	#endregion
	
	public void SwitchState(int addPose)
	{
		
		if(statePos == state.stand && addPose>0)
		{
			animator.SetTrigger("StandToCrouch");
			burnOut = Formulas.BURNOUT_STAND_TO_CROUCH;
		}
		else if(statePos == state.crouch && addPose>0)
		{
			animator.SetTrigger("CrouchToCrawl");
			burnOut = Formulas.BURNOUT_CROUCH_TO_CRAWL;
		}
		else if(statePos == state.crawl && addPose<0)
		{
			animator.SetTrigger("CrawlToCrouch");
			burnOut = Formulas.BURNOUT_CRAWL_TO_CROUCH;
		}
		else if(statePos == state.crouch && addPose<0)
		{
			animator.SetTrigger("CrouchToStand");
			burnOut = Formulas.BURNOUT_CROUCH_TO_STAND;
		}
		else 
			return;
		
		statePos += addPose;
		
		if(statePos == state.stand)
		{
			navMeshAgent.agentTypeID = 0;
			navMeshAgent.speed = Formulas.BASE_SPEED;
			accuracyStateBonus = 1f;
			defenseStateBonus = 1f;
		}
		else if(statePos == state.crouch)
		{
			navMeshAgent.agentTypeID = -1372625422;
			navMeshAgent.speed = Formulas.CROUCH_SPEED;
			accuracyStateBonus = Formulas.ACCURACY_CROUCH_BONUS;
			defenseStateBonus = Formulas.DEFENSE_CROUCH_BONUS;
		}
		else if(statePos == state.crawl)
		{
			navMeshAgent.agentTypeID = -334000983;
			navMeshAgent.speed = Formulas.CRAWL_SPEED;
			accuracyStateBonus = Formulas.ACCURACY_CRAWL_BONUS;
			defenseStateBonus = Formulas.DEFENSE_CRAWL_BONUS;
		}
		vis.BodyPartsResize(statePos);
	}
	
	
	private void ResetPath()
	{
		
		navMeshAgent.velocity = Vector3.zero;
		navMeshAgent.ResetPath();
	}
	
	public void SendAction(int actionType)
	{
		nextAction = actionType;
	}
	
	private void DoAction()
	{
		if(nextAction==0)
		{
			SwitchState(1);
		}
		else if(nextAction==1)
		{
			SwitchState(-1);
		}
		else if(nextAction==2)
		{
			navMeshAgent.SetDestination(destination);
			SetTarget(null);
			//destination = new Vector3(0,0,0);
		}
		else if(nextAction==3)
		{
			ReloadWeapon();
		}
		else if(nextAction==4)
		{
			ShootCheck();
		}
		
		if(destination != new Vector3(0,0,0) && nextAction != 4)
			SendAction(2);
		else if (currentTarget)
			SendAction(4);
		else
			SendAction(255);
	}

}




