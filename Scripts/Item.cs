using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Common_Item", menuName = "Item/Generic")]
public class Item : ScriptableObject 
{

	public new string name;
	public string desc;
	
	public Sprite image;
	
	public bool stackable;
	public int quantity; //?
	

}
