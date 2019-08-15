using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Item/Ammo")]
public class ItemAmmo : Item 
{
	
	public ammoType ammo;
	
}

public enum ammoType{a_9x19mmParabellum, a_7_65x21mmParabellum,
a_32_ACP, a_357_Magnum, a_38_Special, a_44_Magnum, a_50ActionExpress, 
a_5_56, a_5_7x28mm, a_7_62, a_12_7};
