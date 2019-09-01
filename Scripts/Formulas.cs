﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas : MonoBehaviour 
{
	
	public static float VELOCITY_SCALER = 14f;
	public static float RANGE_SCALER = 14f;
	public static float DMG_PERCENTAGE_RANDOM = 10;
	
	public static int LOG_LIMIT = 30;
	
	public static float accuracyBurstPenalty = 0.8f;
	public static float accuracyAutoPenalty = 0.5f;
	
	public static float accuracyCrouchBonus = 1.05f;
	public static float accuracyCrawlBonus = 1.07f;
	
	public static float defenseCrouchBonus = 0.92f;
	public static float defenseCrawlBonus = 0.85f;
	
	public static float legsDmgMultiplier = 0.3f;
	public static float headDmgMultiplier = 2.0f;
	
	
	
	public static float Distance(Transform a, Transform b)
	{
		float distance = Vector3.Distance(a.position, b.position);
		return distance;
	}
	
	public static float ChanceToHit(Stats attacker, Transform defenderPart)
	{
		float distance = Distance(attacker.head.transform, defenderPart);
		ItemWeapon weapon = attacker.weapon;
		part bodyPart = defenderPart.GetComponent<BodyPart>().bodyPart;
		Stats defender = defenderPart.GetComponent<BodyPart>().owner;
		
		float modeP = attacker.accuracyModePenalty;
		float stateB = attacker.accuracyStateBonus;
		float stateP = defender.defenseStateBonus;

		
		//STEP 1: out of range
		if((weapon.effectiveRange / RANGE_SCALER) * 10 < distance)
			return 0;
			
		//STEP 2: formula
		float chanceToHit;
		chanceToHit = (attacker.accuracy*4 * ((float)weapon.accuracy/100+1) / (distance/7)) * modeP * stateB * stateP; 
		
		//STEP 3: 1/2 cth if out of effectiveRange 
		if(weapon.effectiveRange / RANGE_SCALER < distance)
			chanceToHit /= 2;
		
		//STEP 4: 1/2 cth if head
		if(bodyPart == part.head)
			chanceToHit /= 2;
		
		//STEP 5: minor window for miss if cth < 200
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
