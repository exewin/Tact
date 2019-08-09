using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Common Item", menuName = "Item/Generic")]
public class Item : ScriptableObject
{
	
	public new string name;
	[TextArea(4,3)] public string desc;
	public Sprite image;
	public float weight;
	public bool stackable;
	[HideInInspector] public int _quantity = 1;
	
	
	public int quantity
	{
		get { return (stackable ? _quantity : 1);}
		set { _quantity = value; }
	}


}
