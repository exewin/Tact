using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour 
{
	
	public Image icon; //UI Content
	Item info; //Scriptable Object info
	public Text quantity; //Item quantity
	
	public void Assign(Item item)
	{
		info = item;
		icon.sprite = info.image;
		if(item.stackable)
		{
			quantity.gameObject.SetActive(true);
			quantity.text = ""+item.quantity;
		}
	}



}
