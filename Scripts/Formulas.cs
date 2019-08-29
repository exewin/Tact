using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas : MonoBehaviour 
{
	
	public static float VELOCITY_SCALER = 14f;
	public static float RANGE_SCALER = 14f;
	public static float DMG_PERCENTAGE_RANDOM = 10;
	public static int LOG_LIMIT = 30;
	
	
	public static float Distance(Transform a, Transform b)
	{
		float distance = Vector3.Distance(a.position, b.position);
		return distance;
	}
	
	public static float ChanceToHit(float distance, int accuracy, ItemWeapon weapon, part bodyPart)
	{
		if((weapon.effectiveRange / RANGE_SCALER) * 10 < distance)
			return 0;
			
		float chanceToHit = (((float)weapon.accuracy/100+1) * accuracy*4) / (distance/7); 
		
		if(weapon.effectiveRange / RANGE_SCALER < distance)
			chanceToHit /= 2;
		
		if(bodyPart == part.head)
			chanceToHit /= 2;
		
		if(chanceToHit > 200)
			chanceToHit = 100;
		else if(chanceToHit > 95)
			chanceToHit = 95;
		
		return chanceToHit;
	}
	
	public static Vector3 MissedShotRandomizer(float cth, float distance)
	{
		return new Vector3(Random.Range(-4f+(cth/25),4f+(cth/25)),Random.Range(-0.5f+(cth/200),0.5f+(cth/200)),Random.Range(-4f+(cth/25),4f+(cth/25)));
	}
	
	public static int Weight(Stats a)
	{
		//perks?
		return a.strength/2;
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
	
	#region DMG FORMS
	public static int EffectiveRangePowerModifer(float weaponEffectiveRange, int weaponPower, float distance)
	{
		if(weaponEffectiveRange / RANGE_SCALER >= distance)
			return weaponPower;
		else
		{
			float modified = (float)weaponPower / (distance/(weaponEffectiveRange / RANGE_SCALER));
			return (int) modified;
		}
	}
	
	public static int DamageRandomizer(int dmg)
	{
		float rand = Random.Range(-DMG_PERCENTAGE_RANDOM, DMG_PERCENTAGE_RANDOM) / 100;
		rand *= (float)dmg;
		return (int)(dmg + rand);
	}
	
	public static int DamageFormula(int dmg, float dmgMultiplier, float armorDecreaser, int armorFlat)
	{
		float fdmg = Formulas.DamageRandomizer(dmg);
		dmg = (int)(fdmg * dmgMultiplier / armorDecreaser - armorFlat);
		
		if(dmg<0)
			dmg = 0;
		return dmg;
	}
	#endregion
}
