using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Item/Ammo")]
public class ItemAmmo : Item 
{
	
	public ammoType ammo;
	
}

public enum ammoType{a32, a38, a44, a45, a50, a5_56, a7_62, a12_7};
