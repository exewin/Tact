using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Common_Item", menuName = "Item/Generic")]
public class Item : ScriptableObject
{
	
	public new string name;
	[SerializeField]
	[TextArea(4,3)]
	public string desc;
	
	[SerializeField]
	public Sprite image;
	
	[SerializeField]
	public float weigth;
	
	[SerializeField]
	public bool stackable;
	[HideInInspector]
	[SerializeField]
	public int _quantity = 1; //?
	
	
	public int quantity
	{
		get { return (stackable ? _quantity : 1);}
		set { _quantity = value; }
	}


}
