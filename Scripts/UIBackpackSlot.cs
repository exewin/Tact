using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	int index;
	public Image icon; //UI Content
	Item info; //Scriptable Object info
	public Text quantity; //Item quantity
	InventoryController inv;

	
	public void Assign(Item item, int i)
	{
		inv = GameObject.FindWithTag("MainCamera").GetComponent<InventoryController>();
		index = i;
		info = item;
		icon.sprite = info.image;
		if(item.stackable)
		{
			quantity.gameObject.SetActive(true);
			quantity.text = ""+item.quantity;
		}
	}

	
	public void OnPointerEnter (PointerEventData eventData) 
	{
		inv.HoverItemInfo(info);
	}
	
	public void OnPointerDown (PointerEventData eventData) 
	{
		if(Input.GetMouseButtonDown(0))
		{
			inv.EquipItem(index);
		}
		else if(Input.GetMouseButtonDown(1))
		{
			inv.DropItem(index);
		}
	}



}
