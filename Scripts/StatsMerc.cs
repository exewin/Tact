using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMerc : Stats
{
	[SerializeField]
	private GameObject trace; //ItemWeapon?
	
	
	public void Shoot(Transform target)
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
		
		UIControl.UIControl();
		StartCoroutine(ShotDelay(lineDir,distance));
	}
	
	private IEnumerator ShotDelay(Vector3 lineDir,float distance)
	{
		GameObject line = Instantiate(trace,head.position,transform.rotation);
		line.transform.forward = lineDir;
		line.GetComponent<Trace>().Info(weapon.velocity);
		RaycastHit hit;
		
		//real distance ray
		if(Physics.Raycast(head.position, lineDir, out hit))
		{
			float realDistance = Vector3.Distance(head.position,hit.point);
			yield return new WaitForSeconds(realDistance/weapon.velocity);
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
	
	public void ReloadWeapon(ItemAmmo ammo)
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
		UIControl.UIControl();
	}
	
	public void EjectAmmo(ItemAmmo ammo)
	{
		if(!weapon)
			return;
		
		if(ammo.ammo!=weapon.ammo)
			return;
		
		ammo.quantity += weapon.bulletsLeft;
		weapon.bulletsLeft = 0;
		
		audioSource.PlayOneShot(weapon.reloadSound);
		UIControl.UIControl();
	}
	
}
