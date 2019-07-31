using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour 
{
	
	public Image icon; //UI Content
	public Item info; //Scriptable Object info
	
	public void Assign(Item item)
	{
		info = item;
		icon.sprite = info.image;
	}



}
