using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour 
{
	//necessary gameObjects
	public Transform head;
	[SerializeField] private GameObject drop;
	[SerializeField] private GameObject trace; //ItemWeapon?
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
	
	//equipment
	[HideInInspector] public ItemWeapon weapon;
	[HideInInspector] public ItemArmor armor;
	[HideInInspector] public ItemHelmet helmet;
	
	protected AudioSource audioSource;
	
	//UIs
	[SerializeField] protected Log log;
	
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		inv = GetComponent<Inventory>();
		hp = maxHp;
	}
	
	#region equip/unequip
	public void EquipWeapon(Item item)
	{
		if(weapon)
			UnequipWeapon();
		
		inv.RemoveItem(item);
		weapon = (ItemWeapon)item;
		//dmg f
	}
	
	public void EquipArmor(Item item)
	{
		if(armor)
			UnequipArmor();
		
		inv.RemoveItem(item);
		armor = (ItemArmor)item;
		//armor f
	}
	
	public void EquipHelmet(Item item)
	{
		if(helmet)
			UnequipHelmet();
		
		inv.RemoveItem(item);
		helmet = (ItemHelmet)item;
		//armor f
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
	public virtual void Shoot(Transform target)
	{
		if(!weapon)
		{
			Debug.Log("Merc has no weapon!"); //?
			return;
		}
		if(weapon.bulletsLeft == 0)
		{
			if(weapon.ammoUsed && weapon.ammoUsed.quantity>0)
			{
				ReloadWeapon(weapon.ammoUsed);
			}
			else
			{
				Debug.Log("No Ammo!");
				//sound? TODO
			}
			return;
		}
		
		audioSource.PlayOneShot(weapon.shootSound);
		weapon.bulletsLeft--;
		float distance = Formulas.Distance(head, target);
		float chanceToHit = Formulas.ChanceToHit(distance, accuracy, weapon.accuracy);
		
		transform.LookAt(target);
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
		GameObject line = Instantiate(trace,head.position,transform.rotation);
		line.transform.forward = lineDir;
		line.GetComponent<Trace>().Info(weapon.velocity/14f); //velocity const
		RaycastHit hit;
		
		//real distance ray
		if(Physics.Raycast(head.position, lineDir, out hit))
		{
			float realDistance = Vector3.Distance(head.position,hit.point);
			yield return new WaitForSeconds(realDistance/(weapon.velocity/14f)); //velocity const
		}
		
		if(Physics.Raycast(head.position, lineDir, out hit))
		{
			if(hit.collider.tag=="Shootable")
			{
				
				//if(!aimed)
					//Debug.Log("lucky shot"); //LOG?
				
				hit.collider.GetComponent<BodyPart>().Hit(weapon.power); //damage formula
			}
			Destroy(line);
		}
	}
	#endregion
		
	public virtual void Injury(int dmg, part bodyPart)
	{
		//TODO: armor, bodypart
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
			UnequipEverything();
			GameController.RemoveFromList(gameObject); //enemy only!!! TODO
			GameObject dropped = Instantiate(drop, transform.position, Quaternion.identity);
			PickupItem d = dropped.GetComponent<PickupItem>();
			for(int i = 0; i < inv.items.Count; i++)
			{
				d.items.Add(inv.items[i]);
			}
			
			
			Destroy(gameObject);
		}
	}
	
	#region reload/eject
	public virtual void ReloadWeapon(ItemAmmo ammo)
	{
		if(!weapon)
			return;
		
		if(ammo.ammo != weapon.ammo)
			return;
		
		weapon.ammoUsed = ammo;
		int need = weapon.capacity - weapon.bulletsLeft;
		int have = Mathf.Min(ammo.quantity,need);
		weapon.bulletsLeft += have;
		ammo.quantity -= have;
		if(ammo.quantity==0)
		{
			ammo = null;
		}
		
		audioSource.PlayOneShot(weapon.reloadSound);
	}
	
	public virtual void EjectAmmo(ItemAmmo ammo)
	{
		if(!weapon)
			return;
		
		if(ammo.ammo!=weapon.ammo)
			return;
		
		ammo.quantity += weapon.bulletsLeft;
		weapon.bulletsLeft = 0;
		
		audioSource.PlayOneShot(weapon.reloadSound);
	}
	#endregion
}
