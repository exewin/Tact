using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Item/Ammo")]
public class ItemAmmo : Item 
{
	
	public ammoType ammo;
	
}

public enum ammoType 
{
	a_32_ACP,
	a_357_Magnum, 
	a_38_Special, 
	a_44_Magnum, 
	a_45_ACP,
	a_50AE, 
	a_50BMG,
	a_12_Gauge,
	a_20_Gauge,
	a_5_45x39mm,
	a_5_56x45mm, 
	a_7_62x25mmTokarev,
	a_7_62x39mm,
	a_7_62x51mm,
	a_7_62x54mmR, 
	a_9x19mmParabellum
};
