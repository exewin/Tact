using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMerc : Stats
{
	public void Shoot(Transform target)
	{
		//CHECK WEAPON
		//CHECK AMMO
		//CHECK AMMO TYPE TODO
		if(ammo.quantity>0)
		{
			audioSource.PlayOneShot(weapon.shootSound);
			ammo.quantity--; //DO THIS IN INV CONTROLLER
			AccuracyFormula(target);
		}
	}
	
	void AccuracyFormula(Transform target)
	{
		float distance = 2 * Vector3.Distance(target.position, transform.position); //meters
		float chanceToHit = Mathf.Sqrt(weapon.accuracy) * accuracy / distance; 
		
		transform.LookAt(target);
		
		Color col;
		Vector3 lineDir;
		bool aimed;
		
		if(chanceToHit>=Random.Range(1,100))
		{
			col = Color.green;
			lineDir = target.position - transform.position;
			aimed = true;
		}
		else
		{
			col = Color.yellow;
			Vector3 missedShot = new Vector3(Random.Range(-1f,1f),Random.Range(-0.3f,0.3f),Random.Range(-1f,1f));
			lineDir = target.position - transform.position - missedShot;
			aimed = false;
		}
		
		Debug.DrawRay(transform.position, lineDir, col);
		RaycastHit hit;
		Debug.Log("Distance: "+distance+", chance to Hit:"+ chanceToHit+" <"+aimed+">");
		if(Physics.Raycast(transform.position, lineDir, out hit))
		{
			if(hit.collider.tag=="Shootable")
			{
				if(!aimed)
					Debug.Log("lucky shot");
				
				//Debug.Log(hit.collider.name+" hit for "+ weapon.power +" damage");
			}
		}
	}
	
	public int StrengthFormula()
	{
		return strength;
	}
}
