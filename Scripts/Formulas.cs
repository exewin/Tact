using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas : MonoBehaviour 
{
	
	public static float VELOCITY_SCALER = 14f;
	public static float RANGE_SCALER = 14f;
	public static float DMG_PERCENTAGE_RANDOM = 10;
	
	
	public static float Distance(Transform a, Transform b)
	{
		float distance = 2 * Vector3.Distance(a.position, b.position); //x2 convert to meters
		return distance;
	}
	
	public static float ChanceToHit(float distance, int accuracy, int weaponAccuracy)
	{
		//TODO
		//CHECK BODYPART
		float chanceToHit = (weaponAccuracy + accuracy) / (distance/10); 
		
		if(chanceToHit > 200)
			chanceToHit = 100;
		else if(chanceToHit > 95)
			chanceToHit = 95;
		
		return chanceToHit;
	}
	
	public static int Weight(Stats a)
	{
		//perks?
		return a.strength/2;
	}
	
	public static int EffectiveRange(ItemWeapon weapon, float distance)
	{
		Debug.Log("EffectiveRange:"+weapon.effectiveRange/RANGE_SCALER+" / "+"Range:"+distance);
		if(weapon.effectiveRange / RANGE_SCALER >= distance)
			return weapon.power;
		else
		{
			float modified = (float)weapon.power / (distance/(weapon.effectiveRange / RANGE_SCALER));
			return (int) modified;
		}
	}
	
	public static float DefenseFormula(int defense, int dmg)
	{
		if(dmg >= (float)defense * 2f) //100% penetration
			return 1f;
		else
		{
			return (float)defense / 100 + 1;
		}
	}
	
	public static int BaseDamageRandomizer(int dmg)
	{
		float rand = Random.Range(-DMG_PERCENTAGE_RANDOM, DMG_PERCENTAGE_RANDOM) / 100;
		rand *= (float)dmg;
		return (int)(dmg + rand);
	}
	
	public static int DamageFormula(int dmg, float dmgMultiplier, float armorDecreaser, int armorFlat)
	{
		dmg = Formulas.BaseDamageRandomizer(dmg);
		return (int)(dmg * dmgMultiplier / armorDecreaser - armorFlat);
	}

}
