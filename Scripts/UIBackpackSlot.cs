using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	private int index;
	private Item info;
	private InventoryController inv;
	
	[SerializeField] private Image icon;
	[SerializeField] private Text quantity;
	
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
		//remove item? TODO
	}



}
