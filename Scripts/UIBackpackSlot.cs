using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBackpackSlot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	private int index;
	private Item info;
	private InventoryController inv;
	
	private Vector3 iconPos;
	private Transform draggingArea;
	[SerializeField] private Image icon;
	[SerializeField] private Text quantity;
	
	private bool isDragging;
	
	public void Assign(InventoryController controller, Item item, int i, Transform drag)
	{
		draggingArea = drag;
		inv = controller;
		index = i;
		info = item;
		icon.sprite = info.image;
		iconPos = icon.transform.localPosition;
		if(item.stackable)
		{
			quantity.gameObject.SetActive(true);
			quantity.text = ""+item.quantity;
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		inv.HoverItemInfo(info);
	}
	
	public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2) 
		{
			inv.EquipItem(index);
        }
    }
	
	public void OnBeginDrag(PointerEventData eventData)
    {
		icon.transform.SetParent(draggingArea);
		isDragging = true;
    }

	public void OnDrag(PointerEventData eventData)
	{
		icon.transform.position = eventData.position;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		EndDrag();
	}
	
	private void EndDrag()
	{
		isDragging = false;
		icon.transform.SetParent(transform);
		icon.transform.localPosition = iconPos;
		
		for(int i = 0; i<6; i++)
		{
			if(RectTransformUtility.RectangleContainsScreenPoint(inv.mercArea[i], Input.mousePosition, null))
			{
				Destroy(icon);
				inv.GiveItem(index, i);
				inv.RemoveItem(index);
				return;
			}
		}
		
		if(RectTransformUtility.RectangleContainsScreenPoint(inv.weaponArea, Input.mousePosition, null) && info is ItemWeapon
		|| RectTransformUtility.RectangleContainsScreenPoint(inv.armorArea, Input.mousePosition, null) && info is ItemArmor
		|| RectTransformUtility.RectangleContainsScreenPoint(inv.helmetArea, Input.mousePosition, null) && info is ItemHelmet)
		{
			Destroy(icon);
			inv.EquipItem(index);
		}
		else if(RectTransformUtility.RectangleContainsScreenPoint(inv.weaponArea, Input.mousePosition, null) && info is ItemAmmo) // manual drag reload
		{
			inv.ManualReload((ItemAmmo)info);
		}
	}
	
	private void Update()
	{
		if(Input.GetMouseButtonUp(0) && isDragging)
		{
			Debug.Log("jestem tutaj zeby ratowac sytuacje bo debile z unity zjebali OnEndDrag");
			EndDrag();
		}
	}

}
