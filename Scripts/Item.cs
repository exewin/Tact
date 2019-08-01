using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Common_Item", menuName = "Item/Generic")]
public class Item : ScriptableObject 
{

	public new string name;
	public string desc;
	
	public Sprite image;
	
	public float weigth;
	
	public bool stackable;
	[Range(1,999)]
	int _quantity = 1; //?
	
	
	public int quantity
	{
		get { return (stackable ? _quantity : 1);}
		set { _quantity = value; }
	}
	
	
	

}
