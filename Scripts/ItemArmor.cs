﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Item/Armor")]
public class ItemArmor : Item 
{
	
	public int defense;
	public Type type;
	
}

public enum Type{vest, helmet};