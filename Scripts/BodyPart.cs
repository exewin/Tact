using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour 
{
	[SerializeField]
	private part bodyPart;
	[SerializeField]
	private Stats owner;
	
	public void Hit(int dmg)
	{
		owner.Injury(dmg,bodyPart);
	}


}

public enum part {legs,chest,head}