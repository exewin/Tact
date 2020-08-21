using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas : MonoBehaviour 
{
	
	public static float VELOCITY_SCALER = 2f;
	public static float RANGE_SCALER = 14f;
	public static float DMG_PERCENTAGE_RANDOM = 10;
	
	public static int LOG_LIMIT = 30;
	
	public static float BASE_SPEED = 5f;
	public static float CROUCH_SPEED = 2.5f;
	public static float CRAWL_SPEED = 1.3f;
	
	public static float ACCURACY_BURST_PENALTY = 0.8f;
	public static float ACCURACY_AUTO_PENALTY = 0.5f;
	
	public static float ACCURACY_CROUCH_BONUS = 1.05f;
	public static float ACCURACY_CRAWL_BONUS = 1.07f;
	
	public static float DEFENSE_CROUCH_BONUS = 0.92f;
	public static float DEFENSE_CRAWL_BONUS = 0.85f;
	
	public static float LEG_DMG_MULTIPLIER = 0.3f;
	public static float HEAD_DMG_MULTIPLIER = 2.0f;
	
	public static float BURNOUT_STAND_TO_CROUCH = .7f;
	public static float BURNOUT_CROUCH_TO_STAND = .8f;
	public static float BURNOUT_CROUCH_TO_CRAWL = 1.2f;
	public static float BURNOUT_CRAWL_TO_CROUCH = 1.3f;
	
	public static float STAND_HEAVY_RELOAD_TIME = 2f;
	public static float STAND_LIGHT_RELOAD_TIME = 1.5f;
	public static float CROUCH_HEAVY_RELOAD_TIME = 1.8f;
	public static float CROUCH_LIGHT_RELOAD_TIME = 1.3f;
	public static float CRAWL_HEAVY_RELOAD_TIME = 2.7f;
	public static float CRAWL_LIGHT_RELOAD_TIME = 2f;
	
	
	
	
	public static float Distance(Transform a, Transform b)
	{
		if(a&&b) //make sure both are alive?
		{
			float distance = Vector3.Distance(a.position, b.position);
			return distance;
		}
		else 
			return 0;
	}
	
	public static float ChanceToHit(Stats attacker, Transform defenderPart)
	{
		float distance = Distance(attacker.shootPoint.transform, defenderPart);
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
	
	public static Vector3 MissedShotRandomizer(float cth)
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
			return (float)defense / 100f + 1;
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
		float rand = Random.Range(-DMG_PERCENTAGE_RANDOM, DMG_PERCENTAGE_RANDOM) / 100f;
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
