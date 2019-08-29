using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealShoot : MonoBehaviour 
{
	public void Info(Vector3 lineDir, ItemWeapon weapon)
	{
		
		GameObject line = Instantiate(weapon.trace,transform.position,transform.rotation);
		line.transform.forward = lineDir;
		line.GetComponent<Trace>().Info(weapon.velocity/Formulas.VELOCITY_SCALER);
		RaycastHit hit;
		float realDistance = 0;
		
		if(Physics.Raycast(transform.position, lineDir, out hit))
		{
			realDistance = Formulas.Distance(transform, hit.transform);
		}
		StartCoroutine(Delay(lineDir, realDistance, weapon, line));
	}
	
	private IEnumerator Delay(Vector3 lineDir, float realDistance, ItemWeapon weapon, GameObject line)
	{
		
		if(realDistance == 0)
		{
			realDistance = weapon.effectiveRange/Formulas.RANGE_SCALER*10;
		}
		
		float weaponEffectiveRange = weapon.effectiveRange;
		int weaponPower = weapon.power;
		
		yield return new WaitForSeconds(realDistance/(weapon.velocity/Formulas.VELOCITY_SCALER));
		RaycastHit hit;
		if(Physics.Raycast(transform.position, lineDir, out hit, realDistance))
		{
			if(hit.collider.tag=="Shootable")
			{
				
				hit.collider.GetComponent<BodyPart>().Hit(Formulas.EffectiveRangePowerModifer(weaponEffectiveRange, weaponPower, realDistance));
			}
			
		}
		Destroy(line);
		Destroy(gameObject);
	}
}
