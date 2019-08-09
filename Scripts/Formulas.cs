using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas : MonoBehaviour 
{
	
	public static float Distance(Transform a, Transform b)
	{
		float distance = 2 * Vector3.Distance(a.position, b.position); //x2 convert to meters
		return distance;
	}
	
	public static float ChanceToHit(float distance, int accuracy, int weaponAccuracy)
	{
		//TODO
		//CHECK BODYPART

		float chanceToHit = Mathf.Sqrt(weaponAccuracy) * accuracy / distance; 
		
		if(chanceToHit > 100)
			chanceToHit = 100;
		
		return chanceToHit;
	}
	
	public static int Weight(Stats a)
	{
		//perks?
		return a.strength;
	}

}
